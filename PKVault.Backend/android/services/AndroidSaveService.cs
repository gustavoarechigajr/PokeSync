using System.Collections.Concurrent;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using PKHeX.Core;
using PKVault.Backend.android.dto;

namespace PKVault.Backend.android.services;

public class AndroidSaveService(IMemoryCache cache, ILogger<AndroidSaveService> log)
{
    private static readonly TimeSpan SaveTtl = TimeSpan.FromHours(2);

    public AndroidSaveInfoDTO ParseAndCache(string userId, byte[] data, string filename)
    {
        if (!SaveUtil.TryGetSaveFile(data, out var save, filename))
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

        cache.Set(CacheKey(saveId), dto, SaveTtl);

        log.LogInformation(
            "Parsed save {SaveId}: game={Game} gen={Gen} trainer={Trainer} pokemon={Count}",
            saveId, save.Version, save.Generation, save.OT, pokemon.Count
        );

        return dto;
    }

    public AndroidSaveInfoDTO? GetCached(string saveId) =>
        cache.TryGetValue(CacheKey(saveId), out AndroidSaveInfoDTO? dto) ? dto : null;

    private static string CacheKey(string saveId) => $"android-save:{saveId}";

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

                // Stats (calculated from IVs/EVs/nature/level)
                // Use Stat_HP (not Stat_HPMax) — PA8/PLA stores them at different offsets,
                // and SetStats only writes Stat_HP.
                pkm.SetStats(pkm.GetStats(pkm.PersonalInfo));
                var statHp  = (int)pkm.Stat_HP;
                var statAtk = (int)pkm.Stat_ATK;
                var statDef = (int)pkm.Stat_DEF;
                var statSpa = (int)pkm.Stat_SPA;
                var statSpd = (int)pkm.Stat_SPD;
                var statSpe = (int)pkm.Stat_SPE;

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
                    RawData: []
                ));
            }
        }

        return result;
    }
}
