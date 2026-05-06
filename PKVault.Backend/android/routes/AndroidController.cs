using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PKVault.Backend.android.dto;
using PKVault.Backend.android.services;

namespace PKVault.Backend.android.routes;

[Authorize]
[ApiController]
[Route("api/android")]
public class AndroidController(AndroidSaveService saveService) : ControllerBase
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new InvalidOperationException("No user in token");

    /// <summary>
    /// Upload a raw save file (.sav, .srm, etc.).
    /// Returns the full Pokemon list from the save's boxes.
    /// </summary>
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

    /// <summary>
    /// Get Pokemon from a previously uploaded save (by saveId).
    /// </summary>
    [HttpGet("saves/{saveId}")]
    public ActionResult<AndroidSaveInfoDTO> GetSave(string saveId)
    {
        if (!saveId.StartsWith(UserId))
            return Forbid();

        var dto = saveService.GetCached(saveId);
        return dto is null ? NotFound("Save session expired or not found.") : Ok(dto);
    }

    /// <summary>
    /// Get just the Pokemon list from a previously uploaded save.
    /// </summary>
    [HttpGet("saves/{saveId}/pokemon")]
    public ActionResult<List<AndroidPokemonDTO>> GetSavePokemon(string saveId)
    {
        if (!saveId.StartsWith(UserId))
            return Forbid();

        var dto = saveService.GetCached(saveId);
        return dto is null ? NotFound("Save session expired or not found.") : Ok(dto.Pokemon);
    }
}
