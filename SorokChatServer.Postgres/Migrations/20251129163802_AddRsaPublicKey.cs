using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SorokChatServer.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddRsaPublicKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "public_rsa_key",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "public_rsa_key",
                table: "users");
        }
    }
}
