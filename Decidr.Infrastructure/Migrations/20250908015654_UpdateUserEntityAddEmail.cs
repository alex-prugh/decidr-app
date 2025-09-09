using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decidr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntityAddEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "app",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "app",
                table: "Users");
        }
    }
}
