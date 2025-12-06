using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SorokChatServer.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class DiffieHellman : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ephemeral_public_key",
                table: "chats",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "static_public_key",
                table: "chats",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ephemeral_public_key",
                table: "chats");

            migrationBuilder.DropColumn(
                name: "static_public_key",
                table: "chats");
        }
    }
}
