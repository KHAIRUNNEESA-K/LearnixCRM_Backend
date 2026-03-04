using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnixCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class coursetablecreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Course",
                table: "Students",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "CourseInterested",
                table: "Leads",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "CourseInterested",
                table: "LeadHistories",
                newName: "CourseId");

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 3, 3, 18, 12, 58, 258, DateTimeKind.Utc).AddTicks(1726), "$2a$11$Ttn7kTgQ19hjnqDo2sSKfubwTaODrY0wPu5lor.fDFHRpii6QSK2q" });

            migrationBuilder.CreateIndex(
                name: "IX_Leads_CourseId",
                table: "Leads",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leads_Courses_CourseId",
                table: "Leads",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leads_Courses_CourseId",
                table: "Leads");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Leads_CourseId",
                table: "Leads");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Students",
                newName: "Course");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Leads",
                newName: "CourseInterested");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "LeadHistories",
                newName: "CourseInterested");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 3, 2, 9, 9, 15, 603, DateTimeKind.Utc).AddTicks(1758), "$2a$11$8iS3gilE4Ni9laj8yNnF9Ovj4gSAcbm3T/pQnoALh6Nc8kZJZJBNa" });
        }
    }
}
