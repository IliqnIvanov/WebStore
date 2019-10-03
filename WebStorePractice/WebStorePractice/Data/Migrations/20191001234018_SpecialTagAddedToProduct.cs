using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStorePractice.Data.Migrations
{
    public partial class SpecialTagAddedToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpecialTag",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialTag",
                table: "Products");
        }
    }
}
