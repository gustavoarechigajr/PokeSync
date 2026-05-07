using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PKVault.Backend.auth;

#nullable disable

namespace PKVault.Backend.Migrations.Auth;

[DbContext(typeof(AuthDbContext))]
[Migration("20260507000000_AddVaultStats")]
public partial class AddVaultStats : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>("Type1", "AndroidVault", "TEXT", nullable: false, defaultValue: "Normal");
        migrationBuilder.AddColumn<string>("Type2", "AndroidVault", "TEXT", nullable: true);
        migrationBuilder.AddColumn<int>("StatHp",  "AndroidVault", "INTEGER", nullable: false, defaultValue: 0);
        migrationBuilder.AddColumn<int>("StatAtk", "AndroidVault", "INTEGER", nullable: false, defaultValue: 0);
        migrationBuilder.AddColumn<int>("StatDef", "AndroidVault", "INTEGER", nullable: false, defaultValue: 0);
        migrationBuilder.AddColumn<int>("StatSpa", "AndroidVault", "INTEGER", nullable: false, defaultValue: 0);
        migrationBuilder.AddColumn<int>("StatSpd", "AndroidVault", "INTEGER", nullable: false, defaultValue: 0);
        migrationBuilder.AddColumn<int>("StatSpe", "AndroidVault", "INTEGER", nullable: false, defaultValue: 0);
        migrationBuilder.AddColumn<string>("Move1Name", "AndroidVault", "TEXT", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>("Move2Name", "AndroidVault", "TEXT", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>("Move3Name", "AndroidVault", "TEXT", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>("Move4Name", "AndroidVault", "TEXT", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>("Move1Type", "AndroidVault", "TEXT", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>("Move2Type", "AndroidVault", "TEXT", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>("Move3Type", "AndroidVault", "TEXT", nullable: false, defaultValue: "");
        migrationBuilder.AddColumn<string>("Move4Type", "AndroidVault", "TEXT", nullable: false, defaultValue: "");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn("Type1",    "AndroidVault");
        migrationBuilder.DropColumn("Type2",    "AndroidVault");
        migrationBuilder.DropColumn("StatHp",   "AndroidVault");
        migrationBuilder.DropColumn("StatAtk",  "AndroidVault");
        migrationBuilder.DropColumn("StatDef",  "AndroidVault");
        migrationBuilder.DropColumn("StatSpa",  "AndroidVault");
        migrationBuilder.DropColumn("StatSpd",  "AndroidVault");
        migrationBuilder.DropColumn("StatSpe",  "AndroidVault");
        migrationBuilder.DropColumn("Move1Name","AndroidVault");
        migrationBuilder.DropColumn("Move2Name","AndroidVault");
        migrationBuilder.DropColumn("Move3Name","AndroidVault");
        migrationBuilder.DropColumn("Move4Name","AndroidVault");
        migrationBuilder.DropColumn("Move1Type","AndroidVault");
        migrationBuilder.DropColumn("Move2Type","AndroidVault");
        migrationBuilder.DropColumn("Move3Type","AndroidVault");
        migrationBuilder.DropColumn("Move4Type","AndroidVault");
    }
}
