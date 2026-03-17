using LearnixCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnixCRM.Infrastructure.Persistence.Configurations
{
    public class AssignUsersConfiguration
        : IEntityTypeConfiguration<AssignUsers>
    {
        public void Configure(EntityTypeBuilder<AssignUsers> builder)
        {
            builder.ToTable("AssignUsers");

            builder.HasKey(x => x.AssignId);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.Property(x => x.TeamId)
                   .IsRequired();

            builder.Property(x => x.SalesUserId)
                   .IsRequired();

            builder.HasOne(x => x.Team)
                   .WithMany(x => x.Members)
                   .HasForeignKey(x => x.TeamId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SalesUser)
                   .WithMany()
                   .HasForeignKey(x => x.SalesUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.SalesUserId, x.IsActive })
                   .HasDatabaseName("IX_SalesUser_ActiveAssignment");

            builder.Property(x => x.CreatedBy)
                   .HasMaxLength(100);

            builder.Property(x => x.UpdatedBy)
                   .HasMaxLength(100);
        }
    }
}