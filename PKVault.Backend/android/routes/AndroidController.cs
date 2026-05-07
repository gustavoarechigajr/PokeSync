using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PKVault.Backend.android.dto;
using PKVault.Backend.android.services;

namespace PKVault.Backend.android.routes;

[Authorize]
[ApiController]
[Route("api/android")]
public class AndroidController(
    AndroidSaveService saveService,
    AndroidVaultService vaultService) : ControllerBase
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("No user in token");

    // ── Save file endpoints ────────────────────────────────────────────────────

    [HttpPost("saves/upload")]
    [RequestSizeLimit(50 * 1024 * 1024)]
    public async Task<ActionResult<AndroidSaveInfoDTO>> UploadSave(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("File is empty.");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var data = ms.ToArray();

        try
        {
            var result = saveService.ParseAndCache(UserId, data, file.FileName);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
    }

    [HttpGet("saves/{saveId}")]
    public ActionResult<AndroidSaveInfoDTO> GetSave(string saveId)
    {
        if (!saveId.StartsWith(UserId))
            return Forbid();

        var dto = saveService.GetCached(saveId);
        return dto is null ? NotFound("Save session expired or not found.") : Ok(dto);
    }

    [HttpGet("saves/{saveId}/pokemon")]
    public ActionResult<List<AndroidPokemonDTO>> GetSavePokemon(string saveId)
    {
        if (!saveId.StartsWith(UserId))
            return Forbid();

        var dto = saveService.GetCached(saveId);
        return dto is null ? NotFound("Save session expired or not found.") : Ok(dto.Pokemon);
    }

    // ── Vault (permanent bank) endpoints ──────────────────────────────────────

    /// <summary>Returns all Pokémon permanently stored in the user's vault.</summary>
    [HttpGet("vault")]
    public async Task<ActionResult<List<AndroidPokemonDTO>>> GetVault()
    {
        var pokemon = await vaultService.GetVault(UserId);
        return Ok(pokemon);
    }

    /// <summary>
    /// Imports all Pokémon from a cached save session into the vault.
    /// If replace=true, clears the vault first; otherwise appends in new box slots.
    /// </summary>
    [HttpPost("vault/import/{saveId}")]
    public async Task<ActionResult<List<AndroidPokemonDTO>>> ImportToVault(
        string saveId, [FromQuery] bool replace = false)
    {
        if (!saveId.StartsWith(UserId))
            return Forbid();

        var save = saveService.GetCached(saveId);
        if (save is null)
            return NotFound("Save session expired. Re-upload the save file first.");

        var imported = await vaultService.ImportFromSave(UserId, save, replace);
        return Ok(imported);
    }

    /// <summary>Adds a single Pokémon from a cached save session to the vault.</summary>
    [HttpPost("vault/add/{saveId}")]
    public async Task<ActionResult<List<AndroidPokemonDTO>>> AddSingleToVault(
        string saveId, [FromQuery] int box, [FromQuery] int slot)
    {
        if (!saveId.StartsWith(UserId))
            return Forbid();

        var save = saveService.GetCached(saveId);
        if (save is null)
            return NotFound("Save session expired. Re-upload the save file first.");

        var pokemon = save.Pokemon.FirstOrDefault(p => p.Box == box && p.Slot == slot);
        if (pokemon is null)
            return NotFound("No Pokémon at that position.");

        var result = await vaultService.AddSingle(UserId, pokemon);
        return Ok(result);
    }

    /// <summary>Removes a Pokémon from the vault.</summary>
    [HttpDelete("vault/{id}")]
    public async Task<ActionResult> RemoveFromVault(string id)
    {
        var found = await vaultService.Remove(UserId, id);
        return found ? NoContent() : NotFound();
    }

    /// <summary>Moves a vault Pokémon to a different box/slot.</summary>
    [HttpPut("vault/{id}/move")]
    public async Task<ActionResult> MoveVaultPokemon(
        string id, [FromQuery] int box, [FromQuery] int slot)
    {
        await vaultService.Move(UserId, id, box, slot);
        return NoContent();
    }

    /// <summary>
    /// Exports a vault Pokémon into a cached save file at the specified box/slot.
    /// The PKM is converted to the target save's format and injected.
    /// Returns the modified save file for the client to write back to device storage.
    /// </summary>
    /// <summary>
    /// Pre-flight check for ExportToSave. Tells the client whether the transfer will
    /// succeed (errors block, warnings are informational). No state is modified.
    /// </summary>
    [HttpPost("vault/{vaultId}/validate-export")]
    public async Task<ActionResult<TransferValidationDTO>> ValidateExport(
        string vaultId,
        [FromQuery] string saveId)
    {
        if (!saveId.StartsWith(UserId))
            return Forbid();

        var entity = await vaultService.GetEntity(UserId, vaultId);
        if (entity is null)
            return NotFound("Vault Pokémon not found.");

        if (entity.RawData == null || entity.RawData.Length == 0)
            return BadRequest("This Pokémon has no raw data stored. Re-import it from a save file.");

        var pkm = AndroidVaultService.ReconstructPkm(entity);
        if (pkm is null)
            return BadRequest($"Failed to reconstruct PKM from format '{entity.RawDataFormat}'.");

        var result = saveService.ValidateExport(saveId, pkm);
        return Ok(result);
    }

    [HttpPost("vault/{vaultId}/export")]
    public async Task<ActionResult> ExportToSave(
        string vaultId,
        [FromQuery] string saveId,
        [FromQuery] int box,
        [FromQuery] int slot)
    {
        if (!saveId.StartsWith(UserId))
            return Forbid();

        var entity = await vaultService.GetEntity(UserId, vaultId);
        if (entity is null)
            return NotFound("Vault Pokémon not found.");

        if (entity.RawData == null || entity.RawData.Length == 0)
            return BadRequest("This Pokémon has no raw data stored. Re-import it from a save file.");

        var pkm = AndroidVaultService.ReconstructPkm(entity);
        if (pkm is null)
            return BadRequest($"Failed to reconstruct PKM from format '{entity.RawDataFormat}'.");

        var modifiedBytes = saveService.InjectPkm(saveId, pkm, box, slot);
        if (modifiedBytes is null)
            return NotFound("Save session expired. Please re-upload the save file first.");

        return File(modifiedBytes, "application/octet-stream", "save_exported.bin");
    }
}
