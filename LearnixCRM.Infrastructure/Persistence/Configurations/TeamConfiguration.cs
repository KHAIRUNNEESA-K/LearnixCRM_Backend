using LearnixCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnixCRM.Infrastructure.Persistence.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams");

            builder.HasKey(x => x.TeamId);

            builder.Property(x => x.TeamName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.ManagerUserId)
                   .IsRequired();

            builder.HasOne(x => x.ManagerUser)
                   .WithMany()
                   .HasForeignKey(x => x.ManagerUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.IsActive)
                   .IsRequired();
        }
    }
}