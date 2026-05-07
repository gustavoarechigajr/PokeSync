using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PKVault.Backend.auth;

#nullable disable

namespace PKVault.Backend.Migrations.Auth;

[DbContext(typeof(AuthDbContext))]
[Migration("20260506100000_AddAndroidVault")]
public partial class AddAndroidVault : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AndroidVault",
            columns: table => new
            {
                Id = table.Column<string>(type: "TEXT", nullable: false),
                UserId = table.Column<string>(type: "TEXT", nullable: false),
                Box = table.Column<int>(type: "INTEGER", nullable: false),
                Slot = table.Column<int>(type: "INTEGER", nullable: false),
                SpeciesId = table.Column<int>(type: "INTEGER", nullable: false),
                SpeciesName = table.Column<string>(type: "TEXT", nullable: false),
                Nickname = table.Column<string>(type: "TEXT", nullable: false),
                IsNicknamed = table.Column<bool>(type: "INTEGER", nullable: false),
                Level = table.Column<int>(type: "INTEGER", nullable: false),
                IsShiny = table.Column<bool>(type: "INTEGER", nullable: false),
                IsEgg = table.Column<bool>(type: "INTEGER", nullable: false),
                Gender = table.Column<int>(type: "INTEGER", nullable: false),
                Nature = table.Column<string>(type: "TEXT", nullable: false),
                Ball = table.Column<string>(type: "TEXT", nullable: false),
                Generation = table.Column<int>(type: "INTEGER", nullable: false),
                Move1 = table.Column<int>(type: "INTEGER", nullable: false),
                Move2 = table.Column<int>(type: "INTEGER", nullable: false),
                Move3 = table.Column<int>(type: "INTEGER", nullable: false),
                Move4 = table.Column<int>(type: "INTEGER", nullable: false),
                ImportedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
            },
            constraints: table => table.PrimaryKey("PK_AndroidVault", x => x.Id)
        );

        migrationBuilder.CreateIndex(
            name: "IX_AndroidVault_UserId",
            table: "AndroidVault",
            column: "UserId"
        );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "AndroidVault");
    }
}
