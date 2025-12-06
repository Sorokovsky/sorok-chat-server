using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SorokChatServer.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMacSecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mac_secret",
                table: "users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mac_secret",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
