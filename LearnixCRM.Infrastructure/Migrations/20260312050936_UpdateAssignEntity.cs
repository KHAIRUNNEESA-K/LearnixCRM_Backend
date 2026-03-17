using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnixCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssignEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignUsers_Users_ManagerUserId",
                table: "AssignUsers");

            migrationBuilder.DropIndex(
                name: "IX_AssignUsers_ManagerUserId",
                table: "AssignUsers");

            migrationBuilder.DropColumn(
                name: "ManagerUserId",
                table: "AssignUsers");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 3, 12, 5, 9, 35, 983, DateTimeKind.Utc).AddTicks(9728), "$2a$11$BVJchFp9FiFdKeWjQ1o8puznJQb9kdV.UOrU8pq.fai2UinnxJIpC" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManagerUserId",
                table: "AssignUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 3, 10, 12, 14, 49, 838, DateTimeKind.Utc).AddTicks(7867), "$2a$11$Rvv4LaUwGhOjJ32xeSRX../tjfPoMqq3DehBEK4SSq/cMeouK7hSC" });

            migrationBuilder.CreateIndex(
                name: "IX_AssignUsers_ManagerUserId",
                table: "AssignUsers",
                column: "ManagerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignUsers_Users_ManagerUserId",
                table: "AssignUsers",
                column: "ManagerUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
