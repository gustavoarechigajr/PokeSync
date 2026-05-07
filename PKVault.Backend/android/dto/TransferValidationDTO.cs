namespace PKVault.Backend.android.dto;

/// <summary>
/// Result of a pre-transfer compatibility check.
/// Errors block the transfer; warnings are informational only.
/// </summary>
public record TransferValidationDTO(
    bool CanTransfer,
    List<string> Errors,
    List<string> Warnings,
    string? OutputFormat
);
