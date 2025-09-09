using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decidr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCardActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardActivities",
                schema: "app",
                columns: table => new
                {
                    CardId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsLiked = table.Column<bool>(type: "boolean", nullable: false),
                    IsDisliked = table.Column<bool>(type: "boolean", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CardActivity_pkey", x => new { x.CardId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CardActivities_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "app",
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardActivities_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardActivities_UserId",
                schema: "app",
                table: "CardActivities",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardActivities",
                schema: "app");
        }
    }
}
