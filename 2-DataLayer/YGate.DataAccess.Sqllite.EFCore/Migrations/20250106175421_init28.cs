using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGate.DataAccess.Sqllite.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class init28 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataViewerCSStyle",
                table: "CategoryHtmlTemplates");

            migrationBuilder.DropColumn(
                name: "DataViewerTemplate",
                table: "CategoryHtmlTemplates");

            migrationBuilder.DropColumn(
                name: "ShowCSSStyle",
                table: "CategoryHtmlTemplates");

            migrationBuilder.RenameColumn(
                name: "ShowTemplate",
                table: "CategoryHtmlTemplates",
                newName: "Template");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Template",
                table: "CategoryHtmlTemplates",
                newName: "ShowTemplate");

            migrationBuilder.AddColumn<string>(
                name: "DataViewerCSStyle",
                table: "CategoryHtmlTemplates",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataViewerTemplate",
                table: "CategoryHtmlTemplates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShowCSSStyle",
                table: "CategoryHtmlTemplates",
                type: "TEXT",
                nullable: true);
        }
    }
}
