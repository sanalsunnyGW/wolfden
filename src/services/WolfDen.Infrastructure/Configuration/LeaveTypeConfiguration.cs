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
    public class LeaveTypeConfiguration:IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.Property(x => x.Id).HasColumnName("LeaveTypeId");
            builder.Property(x => x.TypeName).HasMaxLength(100);
        }
    }
}
