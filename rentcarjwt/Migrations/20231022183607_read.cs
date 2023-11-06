using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rentcarjwt.Migrations
{
    /// <inheritdoc />
    public partial class read : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "read",
                schema: "ServerRcar",
                table: "Messages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "read",
                schema: "ServerRcar",
                table: "Messages");
        }
    }
}
