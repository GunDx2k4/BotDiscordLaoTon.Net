using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BotDiscordLaoTon.Net.Migrations
{
    /// <inheritdoc />
    public partial class Initv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "CategoryChannelId",
                table: "Boards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryChannelId",
                table: "Boards");
        }
    }
}
