using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGate.DataAccess.Mssql.EFCore.Migrations
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
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedValueGuid",
                table: "CategoryTemplateValues",
                type: "nvarchar(max)",
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
