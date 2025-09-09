using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decidr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSetMemberHasVoted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsUnread",
                schema: "app",
                table: "SetMembers",
                newName: "HasVoted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasVoted",
                schema: "app",
                table: "SetMembers",
                newName: "IsUnread");
        }
    }
}
