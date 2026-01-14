using LearnixCRM.Domain.Entities;
using LearnixCRM.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnixCRM.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.UserId)
                 .ValueGeneratedOnAdd();

            builder.Property(u => u.FullName)
                .HasMaxLength(200);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(u => u.UserRole)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(u => u.Status)
                   .HasConversion<string>()
                   .IsRequired();


            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.CreatedBy)
                .HasMaxLength(100);

            builder.Property(u => u.UpdatedAt);

            builder.Property(u => u.UpdatedBy)
                .HasMaxLength(100);

            builder.Property(u => u.DeletedAt);

            builder.Property(u => u.DeletedBy)
                .HasMaxLength(100);

            builder.HasQueryFilter(u => u.DeletedAt == null);

        }
    }
}
