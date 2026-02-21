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
                   .IsRequired()
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
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(u => u.Status)
                   .HasConversion<int>()
                   .IsRequired();

            builder.HasIndex(u => u.Status);

            builder.Property(u => u.RejectReason)
                   .HasMaxLength(500);

            builder.Property(u => u.EmployeeCode)
                   .HasMaxLength(20);

            builder.HasIndex(u => u.EmployeeCode)
                   .IsUnique()
                   .HasFilter("[EmployeeCode] IS NOT NULL");

            builder.Property(u => u.DateOfJoining);

            builder.Property(u => u.ContactNumber)
                   .HasMaxLength(20);

            builder.HasIndex(u => u.ContactNumber)
                   .IsUnique()
                   .HasFilter("[ContactNumber] IS NOT NULL");

            builder.Property(u => u.ProfileImageUrl)
                   .HasMaxLength(500);

            builder.Property(u => u.ProfileImagePublicId)
                   .HasMaxLength(200);

            builder.Property(u => u.CreatedAt)
                   .IsRequired();

            builder.Property(u => u.CreatedBy)
                   .IsRequired();

            builder.Property(u => u.UpdatedAt);

            builder.Property(u => u.UpdatedBy);

            builder.Property(u => u.DeletedAt);

            builder.Property(u => u.DeletedBy);


            builder.HasQueryFilter(u => u.DeletedAt == null);
        }
    }
}
