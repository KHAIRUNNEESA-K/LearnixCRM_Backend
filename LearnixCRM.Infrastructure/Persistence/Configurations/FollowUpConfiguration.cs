using LearnixCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class FollowUpConfiguration : IEntityTypeConfiguration<FollowUp>
{
    public void Configure(EntityTypeBuilder<FollowUp> builder)
    {
        builder.ToTable("FollowUps");

        builder.HasKey(x => x.FollowUpId);

        builder.Property(x => x.Remark)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.FollowUpDate)
            .IsRequired();

        builder.HasIndex(x => x.LeadId);

        builder.HasOne(x => x.Lead)
            .WithMany(x => x.FollowUps)
            .HasForeignKey(x => x.LeadId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}