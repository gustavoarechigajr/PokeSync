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
    // Extended fields
    string Type1,
    string? Type2,
    int StatHp,
    int StatAtk,
    int StatDef,
    int StatSpa,
    int StatSpd,
    int StatSpe,
    string Move1Name,
    string Move2Name,
    string Move3Name,
    string Move4Name,
    string Move1Type,
    string Move2Type,
    string Move3Type,
    string Move4Type,
    [property: System.Text.Json.Serialization.JsonConverter(typeof(ByteArrayJsonConverter))] byte[] RawData,
    string? RawDataFormat
);

public record AndroidSaveInfoDTO(
    string SaveId,
    string GameVersion,
    byte Generation,
    string TrainerName,
    int BoxCount,
    int BoxSlotCount,
    int PokemonCount,
    List<AndroidPokemonDTO> Pokemon
);
