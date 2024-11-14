using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
   
        public class StatusTypeConfiguration : IEntityTypeConfiguration<StatusType>
        {
            public void Configure(EntityTypeBuilder<StatusType> builder)
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("StatusTypeId");
                builder.Property(x => x.StatusName).HasMaxLength(15);
            }
        }
    
}
