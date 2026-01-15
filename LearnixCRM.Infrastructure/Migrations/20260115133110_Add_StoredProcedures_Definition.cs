using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnixCRM.Infrastructure.Migrations
{
    public partial class Add_StoredProcedures_Definition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Execute(migrationBuilder, "CreatePasswordReset.sql");
            Execute(migrationBuilder, "GetAllUsers.sql");
            Execute(migrationBuilder, "GetInviteByToken.sql");
            Execute(migrationBuilder, "GetLatestPasswordResetByEmail.sql");
            Execute(migrationBuilder, "GetPasswordResetByToken.sql");
            Execute(migrationBuilder, "GetPendingInvites.sql");
            Execute(migrationBuilder, "GetPendingUsers.sql");
            Execute(migrationBuilder, "GetUserByEmail.sql");
            Execute(migrationBuilder, "GetUserForLogin.sql");
            Execute(migrationBuilder, "ResetPassword.sql");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS CreatePasswordReset");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAllUsers");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetInviteByToken");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetLatestPasswordResetByEmail");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPasswordResetByToken");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPendingInvites");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetPendingUsers");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetUserByEmail");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetUserForLogin");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS ResetPassword");
        }

        private void Execute(MigrationBuilder migrationBuilder, string fileName)
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Infrastructure",
                "Persistence",
                "StoredProcedures",
                fileName
            );

            migrationBuilder.Sql(File.ReadAllText(path));
        }
    }
}
