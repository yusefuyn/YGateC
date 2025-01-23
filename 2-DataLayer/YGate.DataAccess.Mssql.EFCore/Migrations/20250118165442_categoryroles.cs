using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGate.DataAccess.Mssql.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class categoryroles : Migration
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

            migrationBuilder.CreateTable(
                name: "CategoryRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DBGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CategoryGuid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleGuid = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryRoles", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryRoles");

            migrationBuilder.RenameColumn(
                name: "Template",
                table: "CategoryHtmlTemplates",
                newName: "ShowTemplate");

            migrationBuilder.AddColumn<string>(
                name: "DataViewerCSStyle",
                table: "CategoryHtmlTemplates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataViewerTemplate",
                table: "CategoryHtmlTemplates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShowCSSStyle",
                table: "CategoryHtmlTemplates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
