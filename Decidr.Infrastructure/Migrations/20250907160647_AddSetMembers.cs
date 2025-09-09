using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Decidr.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSetMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SetMembers",
                schema: "app",
                columns: table => new
                {
                    SetId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsUnread = table.Column<bool>(type: "boolean", nullable: false),
                    AddedById = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SetMembers_pkey", x => new { x.SetId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SetMembers_Sets_SetId",
                        column: x => x.SetId,
                        principalSchema: "app",
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetMembers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "app",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetMembers_UserId",
                schema: "app",
                table: "SetMembers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetMembers",
                schema: "app");
        }
    }
}
