using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class AttendanceStatusConfiguration: IEntityTypeConfiguration<AttendanceStatus>
    {
      
            public void Configure(EntityTypeBuilder<AttendanceStatus> builder)
            {
                builder.HasKey(x => x.Id);
            }
        
    }
}
