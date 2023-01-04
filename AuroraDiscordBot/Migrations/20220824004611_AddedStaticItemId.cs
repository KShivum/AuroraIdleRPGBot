using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraDiscordBot.Migrations
{
    public partial class AddedStaticItemId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StaticItemId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaticItemId",
                table: "Items");
        }
    }
}
