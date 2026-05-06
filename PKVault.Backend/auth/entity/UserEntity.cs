namespace PKVault.Backend.auth.entity;

public class UserEntity
{
    public required string Id { get; init; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
