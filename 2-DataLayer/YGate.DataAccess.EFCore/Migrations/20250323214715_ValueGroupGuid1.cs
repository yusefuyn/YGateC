using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGate.DataAccess.Postgresql.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class ValueGroupGuid1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyGuid",
                table: "EntitiePropertyValues",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedValueGuid",
                table: "CategoryTemplateValues",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyGuid",
                table: "EntitiePropertyValues");

            migrationBuilder.DropColumn(
                name: "SelectedValueGuid",
                table: "CategoryTemplateValues");
        }
    }
}
