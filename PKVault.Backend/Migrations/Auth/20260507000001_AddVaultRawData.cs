using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PKVault.Backend.Migrations.Auth;

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
