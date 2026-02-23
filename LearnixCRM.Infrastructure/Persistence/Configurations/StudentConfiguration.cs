using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(x => x.StudentId);

        builder.Property(x => x.Course)
            .HasConversion<int>();

        builder.HasIndex(x => x.LeadId)
            .IsUnique();

        builder.HasOne(x => x.Lead)
            .WithOne(x => x.Student)
            .HasForeignKey<Student>(x => x.LeadId);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}