namespace PKVault.Backend.auth.dto;

public record RegisterRequest(string Username, string Password);

public record LoginRequest(string Username, string Password);

public record AuthResponse(string Token, string UserId, string Username);
