using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class StatusTypeConfiguration: IEntityTypeConfiguration<StatusType>
    {
      
            public void Configure(EntityTypeBuilder<StatusType> builder)
            {
                builder.HasKey(x => x.Id);
            }
        
    }
}
