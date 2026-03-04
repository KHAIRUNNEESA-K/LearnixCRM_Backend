using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnixCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class coursetablechanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Courses",
                newName: "CourseId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 3, 3, 18, 39, 19, 287, DateTimeKind.Utc).AddTicks(7043), "$2a$11$bPqBWrRwazYISFN4Z16Jp.cpkzlw1a2pbntFpC0RqyknxqflKlOLO" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Courses",
                newName: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 3, 3, 18, 12, 58, 258, DateTimeKind.Utc).AddTicks(1726), "$2a$11$Ttn7kTgQ19hjnqDo2sSKfubwTaODrY0wPu5lor.fDFHRpii6QSK2q" });
        }
    }
}
