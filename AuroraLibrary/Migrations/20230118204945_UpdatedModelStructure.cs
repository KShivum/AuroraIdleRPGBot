using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraLibrary.Migrations
{
    public partial class UpdatedModelStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Atttribute1Label",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Atttribute2Label",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Atttribute3Label",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Atttribute4Label",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "Atttribute1Label",
                table: "ItemModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Atttribute2Label",
                table: "ItemModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Atttribute3Label",
                table: "ItemModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Atttribute4Label",
                table: "ItemModels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rarity",
                table: "ItemModels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Atttribute1Label",
                table: "ItemModels");

            migrationBuilder.DropColumn(
                name: "Atttribute2Label",
                table: "ItemModels");

            migrationBuilder.DropColumn(
                name: "Atttribute3Label",
                table: "ItemModels");

            migrationBuilder.DropColumn(
                name: "Atttribute4Label",
                table: "ItemModels");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "ItemModels");

            migrationBuilder.AddColumn<string>(
                name: "Atttribute1Label",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Atttribute2Label",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Atttribute3Label",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Atttribute4Label",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
