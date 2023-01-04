using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraDiscordBot.Migrations
{
    public partial class Conversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gold = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Experience = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    Strength = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(type: "VARCHAR(255)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Equipped = table.Column<bool>(type: "bit", nullable: false),
                    Atttribute1Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Atttribute1Value = table.Column<int>(type: "int", nullable: false),
                    Atttribute2Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Atttribute2Value = table.Column<int>(type: "int", nullable: false),
                    Atttribute3Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Atttribute3Value = table.Column<int>(type: "int", nullable: false),
                    Atttribute4Label = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Atttribute4Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Players_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_OwnerId",
                table: "Items",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
