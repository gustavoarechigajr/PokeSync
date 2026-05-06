namespace PKVault.Backend.android.dto;

public record AndroidPokemonDTO(
    string Id,
    ushort SpeciesId,
    string SpeciesName,
    string Nickname,
    bool IsNicknamed,
    byte Level,
    bool IsShiny,
    bool IsEgg,
    int Gender,
    string Nature,
    string Ball,
    byte Generation,
    int Box,
    int Slot,
    ushort Move1,
    ushort Move2,
    ushort Move3,
    ushort Move4,
    byte[] RawData
);

public record AndroidSaveInfoDTO(
    string SaveId,
    string GameVersion,
    byte Generation,
    string TrainerName,
    int BoxCount,
    int PokemonCount,
    List<AndroidPokemonDTO> Pokemon
);
