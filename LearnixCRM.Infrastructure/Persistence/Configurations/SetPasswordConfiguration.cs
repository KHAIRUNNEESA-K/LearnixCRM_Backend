using LearnixCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnixCRM.Infrastructure.Configurations
{
    public class SetPasswordConfiguration
        : IEntityTypeConfiguration<UserPasswordToken>
    {
        public void Configure(EntityTypeBuilder<UserPasswordToken> builder)
        {
            builder.ToTable("UserPasswordTokens");

            builder.HasKey(x => x.UserPasswordTokenId);

            builder.Property(x => x.TokenHash)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.TokenType)
                   .IsRequired();

            builder.Property(x => x.Expiry)
                   .IsRequired();

            builder.Property(x => x.IsUsed)
                   .IsRequired();

            builder.Property(x => x.UsedAt)
                   .IsRequired(false);

            builder.HasOne(x => x.User)
                   .WithMany(u => u.PasswordTokens)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.TokenHash)
                   .IsUnique();

            builder.Ignore(x => x.RawToken);
        }
    }
}
