using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuroraAPi.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserNameandXP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscordSnowflake",
                table: "Users",
                type: "numeric(20,0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "XP",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "XP",
                table: "Users");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscordSnowflake",
                table: "Users",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)",
                oldNullable: true);
        }
    }
}
