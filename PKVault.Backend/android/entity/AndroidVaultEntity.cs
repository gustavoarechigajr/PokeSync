namespace PKVault.Backend.android.entity;

public class AndroidVaultEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string UserId { get; set; }
    public int Box { get; set; }
    public int Slot { get; set; }
    public ushort SpeciesId { get; set; }
    public string SpeciesName { get; set; } = "";
    public string Nickname { get; set; } = "";
    public bool IsNicknamed { get; set; }
    public byte Level { get; set; }
    public bool IsShiny { get; set; }
    public bool IsEgg { get; set; }
    public int Gender { get; set; }
    public string Nature { get; set; } = "";
    public string Ball { get; set; } = "";
    public byte Generation { get; set; }
    public ushort Move1 { get; set; }
    public ushort Move2 { get; set; }
    public ushort Move3 { get; set; }
    public ushort Move4 { get; set; }
    public string Type1 { get; set; } = "Normal";
    public string? Type2 { get; set; }
    public int StatHp { get; set; }
    public int StatAtk { get; set; }
    public int StatDef { get; set; }
    public int StatSpa { get; set; }
    public int StatSpd { get; set; }
    public int StatSpe { get; set; }
    public string Move1Name { get; set; } = "";
    public string Move2Name { get; set; } = "";
    public string Move3Name { get; set; } = "";
    public string Move4Name { get; set; } = "";
    public string Move1Type { get; set; } = "";
    public string Move2Type { get; set; } = "";
    public string Move3Type { get; set; } = "";
    public string Move4Type { get; set; } = "";
    public byte[]? RawData { get; set; }
    public string? RawDataFormat { get; set; }
    public int HeldItemId { get; set; }
    public string? HeldItemName { get; set; }
    public DateTime ImportedAt { get; set; } = DateTime.UtcNow;
}
