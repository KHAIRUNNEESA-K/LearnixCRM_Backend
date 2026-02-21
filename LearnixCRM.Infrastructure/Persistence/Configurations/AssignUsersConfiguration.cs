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
    public class AssignUsersConfiguration
        : IEntityTypeConfiguration<AssignUsers>
    {
        public void Configure(EntityTypeBuilder<AssignUsers> builder)
        {

            builder.ToTable("AssignUsers");

            builder.HasKey(x => x.AssignId);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.Property(x => x.SalesUserId)
                   .IsRequired();

            builder.Property(x => x.ManagerUserId)
                   .IsRequired();

            builder.HasOne(x => x.SalesUser)
                   .WithMany()
                   .HasForeignKey(x => x.SalesUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ManagerUser)
                   .WithMany() 
                   .HasForeignKey(x => x.ManagerUserId)
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
