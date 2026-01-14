using LearnixCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnixCRM.Infrastructure.Persistence.Configurations
{
    public class UserInviteConfiguration : IEntityTypeConfiguration<UserInvite>
    {
        public void Configure(EntityTypeBuilder<UserInvite> builder)
        {
            builder.ToTable("UserInvites");

            builder.HasKey(i => i.InviteId);

            builder.Property(i => i.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.Token)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(i => i.Token)
                .IsUnique();

            builder.Property(i => i.ExpiryDate)
                .IsRequired();

            builder.Property(i => i.IsUsed)
                .IsRequired();

            builder.Property(i => i.CreatedAt)
                .IsRequired();

            builder.Property(i => i.CreatedBy)
                .HasMaxLength(100);

            builder.Property(i => i.UpdatedAt);

            builder.Property(i => i.UpdatedBy)
                .HasMaxLength(100);

            builder.Property(i => i.DeletedAt);

            builder.Property(i => i.DeletedBy)
                .HasMaxLength(100);

            builder.HasQueryFilter(i => i.DeletedAt == null);

        }
    }
}
