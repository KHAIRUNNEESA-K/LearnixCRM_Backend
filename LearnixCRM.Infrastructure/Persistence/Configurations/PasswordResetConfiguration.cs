using LearnixCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnixCRM.Infrastructure.Persistence.Configuration
{
    public class PasswordResetConfiguration : IEntityTypeConfiguration<PasswordReset>
    {
        public void Configure(EntityTypeBuilder<PasswordReset> builder)
        {
            builder.ToTable("PasswordResets");

            builder.HasKey(pr => pr.ResetId);

            builder.Property(pr => pr.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(pr => pr.Token)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(pr => pr.ExpiryDate)
                .IsRequired();

            builder.Property(pr => pr.IsUsed)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(pr => pr.CreatedAt)
                .IsRequired();

            builder.Property(pr => pr.CreatedBy)
                .HasMaxLength(200);

            builder.Property(pr => pr.UpdatedAt);

            builder.Property(pr => pr.UpdatedBy)
                .HasMaxLength(200);

            builder.Property(pr => pr.DeletedAt);

            builder.Property(pr => pr.DeletedBy)
                .HasMaxLength(200);

            builder.HasQueryFilter(pr => pr.DeletedAt == null);
        }
    }
}
