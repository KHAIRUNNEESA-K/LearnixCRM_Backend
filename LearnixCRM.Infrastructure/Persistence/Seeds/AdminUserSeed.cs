using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Infrastructure.Persistence.Seed
{
    public static class AdminUserSeed
    {
        public static User CreateAdmin()
        {
            var admin = User.CreateInvitedUser(
                fullName: Guid.NewGuid().ToString(),
                email: "admin@learnixcrm.com",
                role: UserRole.Admin,
                createdBy: "SYSTEM"
            );

            var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");

            admin.Activate(
                passwordHash: passwordHash,
                updatedBy: "SYSTEM"
            );
            return admin;
        }
    }
}
