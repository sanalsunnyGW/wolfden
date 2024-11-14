using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class LeaveRequestConfiguration:IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.HasOne(x => x.Employee).WithMany().HasForeignKey(x =>x.EmployeeId);
            builder.HasOne(x=>x.LeaveType).WithMany().HasForeignKey(x=>x.TypeId);
        }
    }
}
