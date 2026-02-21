using LearnixCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnixCRM.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.RefreshTokenId);

            builder.Property(rt => rt.RefreshTokenId)
                   .ValueGeneratedOnAdd();

            builder.Property(rt => rt.Token)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasIndex(rt => rt.Token)
                   .IsUnique();

            builder.Property(rt => rt.ExpiresAt)
                   .IsRequired();

            builder.Property(rt => rt.IsRevoked)
                   .IsRequired()
                   .HasDefaultValue(false);

            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(rt => rt.CreatedAt)
                   .IsRequired();

            builder.Property(rt => rt.CreatedBy)
                   .HasMaxLength(100);

            builder.Property(rt => rt.UpdatedAt);

            builder.Property(rt => rt.UpdatedBy)
                   .HasMaxLength(100);

            builder.Property(rt => rt.DeletedAt);

            builder.Property(rt => rt.DeletedBy)
                   .HasMaxLength(100);

            builder.HasQueryFilter(rt => rt.DeletedAt == null);
        }
    }
}
