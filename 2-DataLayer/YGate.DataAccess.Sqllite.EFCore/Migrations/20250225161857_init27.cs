using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGate.DataAccess.Sqllite.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class init27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "Roles",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "PropertyGroupValues",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "PropertyGroups",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "MeasurementUnits",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "MeasurementCategories",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "Entities",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "EntitiePropertyValues",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "CategoryTemplateValues",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "CategoryTemplates",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "CategoryRoles",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "CategoryHtmlTemplates",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "Categories",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "AccountsPasswords",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "Accounts",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "AccountRoles",
                newName: "CreatorGuid");

            migrationBuilder.RenameColumn(
                name: "OwnerGuid",
                table: "AccountProperties",
                newName: "CreatorGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "Roles",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "PropertyGroupValues",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "PropertyGroups",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "MeasurementUnits",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "MeasurementCategories",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "Entities",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "EntitiePropertyValues",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "CategoryTemplateValues",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "CategoryTemplates",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "CategoryRoles",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "CategoryHtmlTemplates",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "Categories",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "AccountsPasswords",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "Accounts",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "AccountRoles",
                newName: "OwnerGuid");

            migrationBuilder.RenameColumn(
                name: "CreatorGuid",
                table: "AccountProperties",
                newName: "OwnerGuid");
        }
    }
}
