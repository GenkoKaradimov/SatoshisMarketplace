using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatoshisMarketplace.Entities.Migrations
{
    public partial class M3UsersAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdministator",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdministator",
                table: "Users");
        }
    }
}
