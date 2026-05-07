using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using PKHeX.Core;
using PKVault.Backend.android.dto;

namespace PKVault.Backend.android.services;

public class AndroidSaveService(
    IMemoryCache cache,
    IPkmConvertService pkmConvertService,
    ISettingsService settingsService,
    ILogger<AndroidSaveService> log)
{
    private static readonly TimeSpan SaveTtl = TimeSpan.FromHours(2);
    private static readonly object DiskLock = new();

    private static string SaveDtoKey(string saveId) => $"android-save:dto:{saveId}";
    private static string RawDataKey(string saveId) => $"android-save:raw:{saveId}";
    private static string FilenameKey(string saveId) => $"android-save:filename:{saveId}";

    private string SaveDirectory()
    {
        var dir = settingsService.GetSettings().GetAndroidSavesPath();
        Directory.CreateDirectory(dir);
        return dir;
    }
    private string RawDataPath(string saveId) => Path.Combine(SaveDirectory(), $"{saveId}.sav");
    private string FilenameMetaPath(string saveId) => Path.Combine(SaveDirectory(), $"{saveId}.meta");

    public AndroidSaveInfoDTO ParseAndCache(string userId, byte[] data, string filename)
    {
        // PKHeX decrypts/mutates the input array in place for some formats (SwSh via SwishCrypto).
        // Parse a copy so the cached + disk-persisted bytes remain in their original on-device form,
        // letting future InjectPkm calls re-parse them.
        if (!SaveUtil.TryGetSaveFile((byte[])data.Clone(), out var save, filename))
            throw new InvalidOperationException($"Unrecognized save file format: {filename}");

        var saveId = $"{userId}-{Guid.NewGuid():N}";
        var pokemon = ExtractPokemon(save);

        var dto = new AndroidSaveInfoDTO(
            SaveId: saveId,
            GameVersion: FriendlyGameName(save.Version),
            Generation: save.Generation,
            TrainerName: save.OT,
            BoxCount: save.BoxCount,
            BoxSlotCount: save.BoxSlotCount,
            PokemonCount: pokemon.Count,
            Pokemon: pokemon
        );

        cache.Set(SaveDtoKey(saveId), dto, SaveTtl);
        cache.Set(RawDataKey(saveId), data, SaveTtl);
        cache.Set(FilenameKey(saveId), filename, SaveTtl);
        WriteRawDataToDisk(saveId, data, filename);

        log.LogInformation(
            "Parsed save {SaveId}: game={Game} gen={Gen} trainer={Trainer} pokemon={Count}",
            saveId, save.Version, save.Generation, save.OT, pokemon.Count
        );

        return dto;
    }

    /// <summary>
    /// Returns the parsed DTO for a save. Cache → disk fallthrough — if the in-memory
    /// cache is cold (process restart, TTL expiry), the raw bytes are reloaded from disk
    /// and the DTO is rebuilt.
    /// </summary>
    public AndroidSaveInfoDTO? GetCached(string saveId)
    {
        if (cache.TryGetValue(SaveDtoKey(saveId), out AndroidSaveInfoDTO? dto) && dto != null)
            return dto;

        var disk = ReadRawDataFromDisk(saveId);
        if (disk == null) return null;
        var (rawBytes, filename) = disk.Value;

        if (!SaveUtil.TryGetSaveFile((byte[])rawBytes.Clone(), out var save, filename))
        {
            log.LogWarning(
                "Disk-cached save {SaveId} failed to parse from {Bytes} bytes (filename hint: {Filename}); ignoring",
                saveId, rawBytes.Length, filename);
            return null;
        }

        var pokemon = ExtractPokemon(save);
        var rebuilt = new AndroidSaveInfoDTO(
            SaveId: saveId,
            GameVersion: FriendlyGameName(save.Version),
            Generation: save.Generation,
            TrainerName: save.OT,
            BoxCount: save.BoxCount,
            BoxSlotCount: save.BoxSlotCount,
            PokemonCount: pokemon.Count,
            Pokemon: pokemon
        );

        cache.Set(SaveDtoKey(saveId), rebuilt, SaveTtl);
        cache.Set(RawDataKey(saveId), rawBytes, SaveTtl);
        cache.Set(FilenameKey(saveId), filename, SaveTtl);
        return rebuilt;
    }

    /// <summary>
    /// Injects a PKM into the save at the given box/slot and returns the modified save bytes.
    /// Returns null only if the save raw data is missing from both cache AND disk.
    /// </summary>
    public byte[]? InjectPkm(string saveId, PKM pkm, int targetBox, int targetSlot)
    {
        var data = GetRawData(saveId);
        if (data == null)
        {
            log.LogWarning("InjectPkm: no raw data for save {SaveId} (cache + disk both empty)", saveId);
            return null;
        }
        var (rawBytes, filename) = data.Value;

        if (!SaveUtil.TryGetSaveFile((byte[])rawBytes.Clone(), out var save, filename))
        {
            log.LogWarning(
                "InjectPkm: failed to parse save {SaveId} from {Bytes} bytes (filename hint: {Filename})",
                saveId, rawBytes.Length, filename);
            return null;
        }

        if (targetBox >= save.BoxCount || targetSlot >= save.BoxSlotCount)
            throw new ArgumentOutOfRangeException(
                $"Box/slot {targetBox}/{targetSlot} out of range for save with {save.BoxCount} boxes, {save.BoxSlotCount} slots");

        var converted = pkmConvertService.ConvertTo(
            new ImmutablePKM(pkm),
            save.BlankPKM.GetType(),
            null,
            save);

        save.SetBoxSlotAtIndex(converted.GetMutablePkm(), targetBox, targetSlot);

        var modifiedBytes = save.Write().ToArray();
        cache.Set(RawDataKey(saveId), modifiedBytes, SaveTtl);
        // DTO is now stale — drop so next GetCached re-extracts from the modified bytes.
        cache.Remove(SaveDtoKey(saveId));
        WriteRawDataToDisk(saveId, modifiedBytes, filename);

        return modifiedBytes;
    }

    private (byte[] Bytes, string Filename)? GetRawData(string saveId)
    {
        if (cache.TryGetValue(RawDataKey(saveId), out byte[]? bytes) && bytes != null)
        {
            cache.TryGetValue(FilenameKey(saveId), out string? cachedName);
            return (bytes, cachedName ?? "");
        }

        var disk = ReadRawDataFromDisk(saveId);
        if (disk == null) return null;
        cache.Set(RawDataKey(saveId), disk.Value.Bytes, SaveTtl);
        cache.Set(FilenameKey(saveId), disk.Value.Filename, SaveTtl);
        return disk;
    }

    private void WriteRawDataToDisk(string saveId, byte[] bytes, string filename)
    {
        var path = RawDataPath(saveId);
        var tmp = path + ".tmp";
        var metaPath = FilenameMetaPath(saveId);
        var metaTmp = metaPath + ".tmp";
        try
        {
            // Write-then-rename: avoids leaving a half-written file if the process dies mid-write.
            // Write the .meta sidecar first — if .sav is present without .meta we fall back to "",
            // which is the old (broken-for-emulator-saves) behavior; never the inverse.
            lock (DiskLock)
            {
                File.WriteAllText(metaTmp, filename ?? "");
                if (File.Exists(metaPath)) File.Delete(metaPath);
                File.Move(metaTmp, metaPath);

                File.WriteAllBytes(tmp, bytes);
                if (File.Exists(path)) File.Delete(path);
                File.Move(tmp, path);
            }
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Failed to persist save raw data to disk: {Path}", path);
            throw;
        }
    }

    private (byte[] Bytes, string Filename)? ReadRawDataFromDisk(string saveId)
    {
        var path = RawDataPath(saveId);
        if (!File.Exists(path)) return null;
        try
        {
            var bytes = File.ReadAllBytes(path);
            var metaPath = FilenameMetaPath(saveId);
            // Pre-meta-sidecar saves on disk: filename unknown — fall back to "".
            var filename = File.Exists(metaPath) ? File.ReadAllText(metaPath) : "";
            return (bytes, filename);
        }
        catch (Exception ex)
        {
            log.LogWarning(ex, "Failed to read save raw data from disk: {Path}", path);
            return null;
        }
    }

    private static string FriendlyGameName(GameVersion v) => v switch
    {
        GameVersion.RD => "Pokémon Red",
        GameVersion.BU => "Pokémon Blue (JP)",
        GameVersion.GN => "Pokémon Green",
        GameVersion.YW => "Pokémon Yellow",
        GameVersion.GD => "Pokémon Gold",
        GameVersion.SV => "Pokémon Silver",
        GameVersion.C  => "Pokémon Crystal",
        GameVersion.R  => "Pokémon Ruby",
        GameVersion.S  => "Pokémon Sapphire",
        GameVersion.E  => "Pokémon Emerald",
        GameVersion.FR => "Pokémon FireRed",
        GameVersion.LG => "Pokémon LeafGreen",
        GameVersion.D  => "Pokémon Diamond",
        GameVersion.P  => "Pokémon Pearl",
        GameVersion.Pt => "Pokémon Platinum",
        GameVersion.HG => "Pokémon HeartGold",
        GameVersion.SS => "Pokémon SoulSilver",
        GameVersion.B  => "Pokémon Black",
        GameVersion.W  => "Pokémon White",
        GameVersion.B2 => "Pokémon Black 2",
        GameVersion.W2 => "Pokémon White 2",
        GameVersion.X  => "Pokémon X",
        GameVersion.Y  => "Pokémon Y",
        GameVersion.OR => "Pokémon Omega Ruby",
        GameVersion.AS => "Pokémon Alpha Sapphire",
        GameVersion.SN => "Pokémon Sun",
        GameVersion.MN => "Pokémon Moon",
        GameVersion.US => "Pokémon Ultra Sun",
        GameVersion.UM => "Pokémon Ultra Moon",
        GameVersion.GP => "Pokémon: Let's Go, Pikachu!",
        GameVersion.GE => "Pokémon: Let's Go, Eevee!",
        GameVersion.SW => "Pokémon Sword",
        GameVersion.SH => "Pokémon Shield",
        GameVersion.BD => "Pokémon Brilliant Diamond",
        GameVersion.SP => "Pokémon Shining Pearl",
        GameVersion.PLA => "Pokémon Legends: Arceus",
        GameVersion.SL => "Pokémon Scarlet",
        GameVersion.VL => "Pokémon Violet",
        _ => v.ToString(),
    };

    private static List<AndroidPokemonDTO> ExtractPokemon(SaveFile save)
    {
        var result = new List<AndroidPokemonDTO>();
        var strings = GameInfo.GetStrings(GameInfo.CurrentLanguage);

        for (int box = 0; box < save.BoxCount; box++)
        {
            for (int slot = 0; slot < save.BoxSlotCount; slot++)
            {
                var pkm = save.GetBoxSlotAtIndex(box, slot);

                if (pkm.Species == 0 || pkm.IsEgg)
                    continue;

                var id = $"{box}-{slot}-{pkm.Species}-{pkm.PID:X8}";
                var speciesName = strings.Species.Count > pkm.Species
                    ? strings.Species[pkm.Species]
                    : pkm.Species.ToString();

                // Types
                var type1Id = pkm.PersonalInfo.Type1;
                var type2Id = pkm.PersonalInfo.Type2;
                var type1 = type1Id < strings.types.Length ? strings.types[type1Id] : "Normal";
                var type2 = type2Id != type1Id && type2Id < strings.types.Length
                    ? strings.types[type2Id] : (string?)null;

                // Read from the calculated array directly — avoids SetStats/Stat_HPMax
                // roundtrip issues on PA8 (PLA) where Stat_HPMax may be stored separately.
                var calcStats = pkm.GetStats(pkm.PersonalInfo);
                var statHp  = (int)calcStats[0];
                var statAtk = (int)calcStats[1];
                var statDef = (int)calcStats[2];
                var statSpa = (int)calcStats[3];
                var statSpd = (int)calcStats[4];
                var statSpe = (int)calcStats[5];

                // Move names & types
                string MoveName(ushort move) =>
                    move > 0 && move < strings.movelist.Length ? strings.movelist[move] : "";
                string MoveType(ushort move)
                {
                    if (move == 0) return "";
                    try
                    {
                        var tid = MoveInfo.GetType(move, pkm.Context);
                        return tid < strings.types.Length ? strings.types[tid] : "Normal";
                    }
                    catch { return "Normal"; }
                }

                result.Add(new AndroidPokemonDTO(
                    Id: id,
                    SpeciesId: pkm.Species,
                    SpeciesName: speciesName,
                    Nickname: pkm.Nickname,
                    IsNicknamed: pkm.IsNicknamed,
                    Level: pkm.CurrentLevel,
                    IsShiny: pkm.IsShiny,
                    IsEgg: pkm.IsEgg,
                    Gender: pkm.Gender,
                    Nature: pkm.StatNature.ToString(),
                    Ball: ((Ball)pkm.Ball).ToString(),
                    Generation: pkm.Format,
                    Box: box,
                    Slot: slot,
                    Move1: pkm.Move1,
                    Move2: pkm.Move2,
                    Move3: pkm.Move3,
                    Move4: pkm.Move4,
                    Type1: type1,
                    Type2: type2,
                    StatHp: statHp,
                    StatAtk: statAtk,
                    StatDef: statDef,
                    StatSpa: statSpa,
                    StatSpd: statSpd,
                    StatSpe: statSpe,
                    Move1Name: MoveName(pkm.Move1),
                    Move2Name: MoveName(pkm.Move2),
                    Move3Name: MoveName(pkm.Move3),
                    Move4Name: MoveName(pkm.Move4),
                    Move1Type: MoveType(pkm.Move1),
                    Move2Type: MoveType(pkm.Move2),
                    Move3Type: MoveType(pkm.Move3),
                    Move4Type: MoveType(pkm.Move4),
                    RawData: pkm.Data.ToArray(),
                    RawDataFormat: pkm.GetType().Name
                ));
            }
        }

        return result;
    }
}
