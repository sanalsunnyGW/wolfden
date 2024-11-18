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
    public class LeaveSettingConfiguration:IEntityTypeConfiguration<LeaveSetting>
    {
        public void Configure(EntityTypeBuilder<LeaveSetting> builder)
        {
            builder.Property(x => x.Id).HasColumnName("LeaveSettingId");
        }
    }
}
