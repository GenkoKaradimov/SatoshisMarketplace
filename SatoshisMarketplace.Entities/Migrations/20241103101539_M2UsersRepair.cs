using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatoshisMarketplace.Entities.Migrations
{
    public partial class M2UsersRepair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "UserLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "UserLogs");
        }
    }
}
