using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGate.DataAccess.Sqllite.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class init23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryDBGuid",
                table: "CategoryTemplates");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "CategoryTemplates",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "CategoryTemplates");

            migrationBuilder.AddColumn<string>(
                name: "CategoryDBGuid",
                table: "CategoryTemplates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
