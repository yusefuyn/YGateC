using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YGate.DataAccess.Sqllite.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class ValueGroupGuid2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PropertyDBGuid",
                table: "EntitiePropertyValues",
                newName: "PropertyGroupGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PropertyGroupGuid",
                table: "EntitiePropertyValues",
                newName: "PropertyDBGuid");
        }
    }
}
