using LearnixCRM.Domain.Entities;
using LearnixCRM.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace LearnixCRM.Infrastructure.Persistence.Context
{
    public class LearnixDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public DbSet<UserPasswordToken> UserPasswordTokens => Set<UserPasswordToken>();

        public DbSet<AssignUsers> AssignUsers => Set<AssignUsers>();

        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public DbSet<Lead> Leads => Set<Lead>();

        public DbSet<LeadHistory> LeadHistories => Set<LeadHistory>();

        public DbSet<FollowUp> FollowUps => Set<FollowUp>();

        public DbSet<Student> Students => Set<Student>();

        public DbSet<Blacklist> Blacklists => Set<Blacklist>();

        public LearnixDbContext(DbContextOptions<LearnixDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LearnixDbContext).Assembly);
            modelBuilder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
            modelBuilder.Entity<UserPasswordToken>().HasQueryFilter(upt => upt.DeletedAt == null);
            modelBuilder.Entity<AssignUsers>().HasQueryFilter(s => s.DeletedAt == null);
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var admin = AdminUserSeed.CreateAdmin();

            modelBuilder.Entity<User>().HasData(new
            {
                UserId = 1,
                FullName = admin.FullName,
                Email = admin.Email,
                PasswordHash = admin.PasswordHash,
                UserRole = admin.UserRole,
                Status = admin.Status,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,  

                UpdatedAt = (DateTime?)null,
                UpdatedBy = (int?)null,

                DeletedAt = (DateTime?)null,
                DeletedBy = (int?)null
            });
        }
    }
}
