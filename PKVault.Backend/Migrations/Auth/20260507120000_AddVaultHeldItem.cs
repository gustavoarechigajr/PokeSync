using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PKVault.Backend.auth;

#nullable disable

namespace PKVault.Backend.Migrations.Auth;

[DbContext(typeof(AuthDbContext))]
[Migration("20260507120000_AddVaultHeldItem")]
public partial class AddVaultHeldItem : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>("HeldItemId",      "AndroidVault", "INTEGER", nullable: false, defaultValue: 0);
        migrationBuilder.AddColumn<string>("HeldItemName", "AndroidVault", "TEXT",    nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn("HeldItemId",   "AndroidVault");
        migrationBuilder.DropColumn("HeldItemName", "AndroidVault");
    }
}
