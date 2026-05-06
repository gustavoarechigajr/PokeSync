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
        var save = SaveUtil.GetVariantSAV(data)
            ?? throw new InvalidOperationException($"Unrecognized save file format: {filename}");

        var saveId = $"{userId}-{Guid.NewGuid():N}";
        var pokemon = ExtractPokemon(save);

        var dto = new AndroidSaveInfoDTO(
            SaveId: saveId,
            GameVersion: save.Version.ToString(),
            Generation: save.Generation,
            TrainerName: save.OT,
            BoxCount: save.BoxCount,
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

                result.Add(new AndroidPokemonDTO(
                    Id: id,
                    SpeciesId: pkm.Species,
                    SpeciesName: speciesName,
                    Nickname: pkm.Nickname,
                    IsNicknamed: pkm.IsNicknamed,
                    Level: pkm.Stat_Level,
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
                    RawData: pkm.DecryptedBoxData
                ));
            }
        }

        return result;
    }
}
