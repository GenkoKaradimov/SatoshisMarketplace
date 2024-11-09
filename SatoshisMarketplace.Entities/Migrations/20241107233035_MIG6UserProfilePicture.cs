using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SatoshisMarketplace.Entities.Migrations
{
    public partial class MIG6UserProfilePicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "Users",
                type: "varbinary(max)",
                maxLength: 1048576,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Users");
        }
    }
}
