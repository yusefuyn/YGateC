using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGate.DataAccess.Mssql.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class init28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntitieOwner",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntitieGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldOwnerGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewOwnerGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTimeUTC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
