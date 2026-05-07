using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PKVault.Backend.auth;

#nullable disable

namespace PKVault.Backend.Migrations.Auth;

[DbContext(typeof(AuthDbContext))]
[Migration("20260507000001_AddVaultRawData")]
public partial class AddVaultRawData : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<byte[]>("RawData",       "AndroidVault", "BLOB", nullable: true);
        migrationBuilder.AddColumn<string>("RawDataFormat", "AndroidVault", "TEXT", nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn("RawData",       "AndroidVault");
        migrationBuilder.DropColumn("RawDataFormat", "AndroidVault");
    }
}
