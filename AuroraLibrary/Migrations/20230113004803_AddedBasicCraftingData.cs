using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraLibrary.Migrations
{
    public partial class AddedBasicCraftingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaticItemId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Items",
                newName: "ModelId");

            migrationBuilder.AddColumn<int>(
                name: "CraftingXP",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ItemModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemRecipies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemRecipies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemRecipies_ItemModels_CreatedItemId",
                        column: x => x.CreatedItemId,
                        principalTable: "ItemModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ModelId",
                table: "Items",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemModels_RecipeId",
                table: "ItemModels",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemRecipies_CreatedItemId",
                table: "ItemRecipies",
                column: "CreatedItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemModels_ModelId",
                table: "Items",
                column: "ModelId",
                principalTable: "ItemModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemModels_ItemRecipies_RecipeId",
                table: "ItemModels",
                column: "RecipeId",
                principalTable: "ItemRecipies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemModels_ModelId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemModels_ItemRecipies_RecipeId",
                table: "ItemModels");

            migrationBuilder.DropTable(
                name: "ItemRecipies");

            migrationBuilder.DropTable(
                name: "ItemModels");

            migrationBuilder.DropIndex(
                name: "IX_Items_ModelId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CraftingXP",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "Items",
                newName: "Type");

            migrationBuilder.AddColumn<int>(
                name: "StaticItemId",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
