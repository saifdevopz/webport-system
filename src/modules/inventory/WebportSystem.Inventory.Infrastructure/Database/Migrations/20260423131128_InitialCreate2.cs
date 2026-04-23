using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebportSystem.Inventory.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "business_profiles",
                schema: "inventory",
                newName: "BusinessProfiles",
                newSchema: "inventory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "BusinessProfiles",
                schema: "inventory",
                newName: "business_profiles",
                newSchema: "inventory");
        }
    }
}
