using LearnixCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class LeadConfiguration : IEntityTypeConfiguration<Lead>
{
    public void Configure(EntityTypeBuilder<Lead> builder)
    {
        builder.ToTable("Leads");

        builder.HasKey(x => x.LeadId);

        builder.Property(x => x.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasMaxLength(20);

        builder.Property(x => x.Remark)
            .HasMaxLength(500);

        builder.Property(x => x.Status).HasConversion<int>();

        builder.Property(x => x.Source).HasConversion<int>();

        builder.Property(x => x.CourseInterested).HasConversion<int>();

        builder.HasMany(x => x.FollowUps)
            .WithOne(x => x.Lead)
            .HasForeignKey(x => x.LeadId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Histories)
            .WithOne(x => x.Lead)
            .HasForeignKey(x => x.LeadId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Student)
            .WithOne(x => x.Lead)
            .HasForeignKey<Student>(x => x.LeadId);

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasIndex(x => x.Phone)
            .IsUnique()
            .HasFilter("[Phone] IS NOT NULL");

        builder.HasIndex(x => x.AssignedToUserId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
