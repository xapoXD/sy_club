using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace club.soundyard.web.Data.Migrations
{
    public partial class Roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the Agreement column to the AspNetRoles table
            migrationBuilder.AddColumn<string>(
                name: "Agreement",
                table: "AspNetRoles",
                type: "nvarchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the Agreement column from the AspNetRoles table
            migrationBuilder.DropColumn(
                name: "Agreement",
                table: "AspNetRoles");
        }
    }
}
