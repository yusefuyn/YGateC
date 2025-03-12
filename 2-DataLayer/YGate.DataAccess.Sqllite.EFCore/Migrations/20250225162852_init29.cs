using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGate.DataAccess.Sqllite.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class init29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntitieOwner",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntitieGuid = table.Column<string>(type: "TEXT", nullable: false),
                    OldOwnerGuid = table.Column<string>(type: "TEXT", nullable: false),
                    NewOwnerGuid = table.Column<string>(type: "TEXT", nullable: false),
                    DateTimeUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntitieOwner", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntitieOwner");
        }
    }
}
