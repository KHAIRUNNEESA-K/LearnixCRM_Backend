using LearnixCRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnixCRM.Infrastructure.Persistence.Configurations
{
    public class LeadHistoryConfiguration : IEntityTypeConfiguration<LeadHistory>
    {
        public void Configure(EntityTypeBuilder<LeadHistory> builder)
        {
            builder.ToTable("LeadHistories");

            builder.HasKey(x => x.HistoryId);

            builder.Property(x => x.Action)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CourseInterested).HasConversion<int>();

            builder.Property(x => x.Source).HasConversion<int>();

            builder.Property(x => x.Status).HasConversion<int>();

            builder.HasIndex(x => x.LeadId);

            builder.HasOne(x => x.Lead)
                .WithMany(x => x.Histories)
                .HasForeignKey(x => x.LeadId);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
