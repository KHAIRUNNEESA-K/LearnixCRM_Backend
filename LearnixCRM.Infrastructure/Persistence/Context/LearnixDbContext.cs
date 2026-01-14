using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using LearnixCRM.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace LearnixCRM.Infrastructure.Persistence
{
    public class LearnixDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<UserInvite> UserInvites => Set<UserInvite>();

        public DbSet<PasswordReset> PasswordResets => Set<PasswordReset>();

        public LearnixDbContext(DbContextOptions<LearnixDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearnixDbContext).Assembly);
            modelBuilder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
            modelBuilder.Entity<UserInvite>().HasQueryFilter(x => x.DeletedAt == null);
            modelBuilder.Entity<PasswordReset>().HasQueryFilter(pr => pr.DeletedAt == null);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new
            {
                UserId = 1, 
                FullName = "System Administrator",
                Email = "admin@learnixcrm.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                UserRole = UserRole.Admin,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "SYSTEM",
                UpdatedAt = (DateTime?)null,
                UpdatedBy = (string?)null,
                DeletedAt = (DateTime?)null,
                DeletedBy = (string?)null
            });
        }

    }
}
