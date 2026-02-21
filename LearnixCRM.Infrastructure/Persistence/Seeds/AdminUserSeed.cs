using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;

namespace LearnixCRM.Infrastructure.Persistence.Seed
{
    public static class AdminUserSeed
    {
        public static User CreateAdmin()
        {
            return User.CreateAdmin(
                email: "admin@learnixcrm.com",
                fullName: "Administrator",
                password: "Admin@123"
            );
        }
    }
}
