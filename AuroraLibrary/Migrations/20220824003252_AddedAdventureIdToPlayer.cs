using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraDiscordBot.Migrations
{
    public partial class AddedAdventureIdToPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdventureId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdventureId",
                table: "Players");
        }
    }
}
