using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGate.DataAccess.Sqllite.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class htmltemplate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CSStyle",
                table: "CategoryHtmlTemplates",
                newName: "ShowCSSStyle");

            migrationBuilder.AddColumn<string>(
                name: "DataViewerCSStyle",
                table: "CategoryHtmlTemplates",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShowTemplate",
                table: "CategoryHtmlTemplates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataViewerCSStyle",
                table: "CategoryHtmlTemplates");

            migrationBuilder.DropColumn(
                name: "ShowTemplate",
                table: "CategoryHtmlTemplates");

            migrationBuilder.RenameColumn(
                name: "ShowCSSStyle",
                table: "CategoryHtmlTemplates",
                newName: "CSStyle");
        }
    }
}
