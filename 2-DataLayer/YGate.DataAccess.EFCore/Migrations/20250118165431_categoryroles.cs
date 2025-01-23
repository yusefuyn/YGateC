using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YGate.DataAccess.Postgresql.EFCore.Migrations
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerGuid = table.Column<string>(type: "text", nullable: false),
                    DBGuid = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CategoryGuid = table.Column<string>(type: "text", nullable: false),
                    RoleGuid = table.Column<string>(type: "text", nullable: false)
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
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DataViewerTemplate",
                table: "CategoryHtmlTemplates",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShowCSSStyle",
                table: "CategoryHtmlTemplates",
                type: "text",
                nullable: true);
        }
    }
}
