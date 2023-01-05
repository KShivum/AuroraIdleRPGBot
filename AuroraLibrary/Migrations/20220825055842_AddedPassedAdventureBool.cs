using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraDiscordBot.Migrations
{
    public partial class AddedPassedAdventureBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PassedAdventure",
                table: "Players",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassedAdventure",
                table: "Players");
        }
    }
}
