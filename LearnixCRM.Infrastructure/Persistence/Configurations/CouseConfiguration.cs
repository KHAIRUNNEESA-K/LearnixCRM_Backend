using LearnixCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnixCRM.Infrastructure.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
         
            builder.HasKey(c => c.CourseId);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.Fee)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(c => c.CourseDuration)
                   .IsRequired();

            builder.Property(c => c.IsActive)
                   .HasDefaultValue(true);

            builder.HasMany(c => c.Leads)
                   .WithOne(l => l.Course)
                   .HasForeignKey(l => l.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}