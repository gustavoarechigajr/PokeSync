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
    public DateTime ImportedAt { get; set; } = DateTime.UtcNow;
}
