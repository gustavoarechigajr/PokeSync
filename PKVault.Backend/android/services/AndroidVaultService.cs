using Microsoft.EntityFrameworkCore;
using PKVault.Backend.android.dto;
using PKVault.Backend.android.entity;
using PKVault.Backend.auth;

namespace PKVault.Backend.android.services;

public class AndroidVaultService(AuthDbContext db)
{
    public async Task<List<AndroidPokemonDTO>> GetVault(string userId)
    {
        var entities = await db.AndroidVault
            .Where(v => v.UserId == userId)
            .OrderBy(v => v.Box).ThenBy(v => v.Slot)
            .ToListAsync();

        return entities.Select(ToDto).ToList();
    }

    public async Task<List<AndroidPokemonDTO>> ImportFromSave(
        string userId, AndroidSaveInfoDTO save, bool replaceExisting)
    {
        if (replaceExisting)
        {
            // Each save overwrites Pokémon previously imported from the same logical save
            // We identify them by species+slot combo — simpler: just clear and re-import
            var existing = db.AndroidVault.Where(v => v.UserId == userId);
            db.AndroidVault.RemoveRange(existing);
        }

        // Find next available box range (start after highest occupied box)
        var maxBox = replaceExisting ? -1 :
            (await db.AndroidVault
                .Where(v => v.UserId == userId)
                .MaxAsync(v => (int?)v.Box) ?? -1);

        var baseBox = maxBox + 1;

        var entities = save.Pokemon.Select(p => new AndroidVaultEntity
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            Box = baseBox + p.Box,
            Slot = p.Slot,
            SpeciesId = (ushort)p.SpeciesId,
            SpeciesName = p.SpeciesName,
            Nickname = p.Nickname,
            IsNicknamed = p.IsNicknamed,
            Level = (byte)p.Level,
            IsShiny = p.IsShiny,
            IsEgg = p.IsEgg,
            Gender = p.Gender,
            Nature = p.Nature,
            Ball = p.Ball,
            Generation = p.Generation,
            Move1 = p.Move1,
            Move2 = p.Move2,
            Move3 = p.Move3,
            Move4 = p.Move4,
            Type1 = p.Type1,
            Type2 = p.Type2,
            StatHp = p.StatHp,
            StatAtk = p.StatAtk,
            StatDef = p.StatDef,
            StatSpa = p.StatSpa,
            StatSpd = p.StatSpd,
            StatSpe = p.StatSpe,
            Move1Name = p.Move1Name,
            Move2Name = p.Move2Name,
            Move3Name = p.Move3Name,
            Move4Name = p.Move4Name,
            Move1Type = p.Move1Type,
            Move2Type = p.Move2Type,
            Move3Type = p.Move3Type,
            Move4Type = p.Move4Type,
        }).ToList();

        db.AndroidVault.AddRange(entities);
        await db.SaveChangesAsync();

        return entities.Select(ToDto).ToList();
    }

    public async Task<bool> Remove(string userId, string id)
    {
        var entity = await db.AndroidVault.FirstOrDefaultAsync(v => v.UserId == userId && v.Id == id);
        if (entity is null) return false;
        db.AndroidVault.Remove(entity);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task Move(string userId, string id, int newBox, int newSlot)
    {
        var entity = await db.AndroidVault.FirstOrDefaultAsync(v => v.UserId == userId && v.Id == id);
        if (entity is null) return;
        entity.Box = newBox;
        entity.Slot = newSlot;
        await db.SaveChangesAsync();
    }

    private static AndroidPokemonDTO ToDto(AndroidVaultEntity v) => new(
        Id: v.Id,
        SpeciesId: v.SpeciesId,
        SpeciesName: v.SpeciesName,
        Nickname: v.Nickname,
        IsNicknamed: v.IsNicknamed,
        Level: v.Level,
        IsShiny: v.IsShiny,
        IsEgg: v.IsEgg,
        Gender: v.Gender,
        Nature: v.Nature,
        Ball: v.Ball,
        Generation: v.Generation,
        Box: v.Box,
        Slot: v.Slot,
        Move1: v.Move1,
        Move2: v.Move2,
        Move3: v.Move3,
        Move4: v.Move4,
        Type1: v.Type1,
        Type2: v.Type2,
        StatHp: v.StatHp,
        StatAtk: v.StatAtk,
        StatDef: v.StatDef,
        StatSpa: v.StatSpa,
        StatSpd: v.StatSpd,
        StatSpe: v.StatSpe,
        Move1Name: v.Move1Name,
        Move2Name: v.Move2Name,
        Move3Name: v.Move3Name,
        Move4Name: v.Move4Name,
        Move1Type: v.Move1Type,
        Move2Type: v.Move2Type,
        Move3Type: v.Move3Type,
        Move4Type: v.Move4Type,
        RawData: []
    );
}
