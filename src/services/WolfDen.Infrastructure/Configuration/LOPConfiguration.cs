using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class LOPConfiguration : IEntityTypeConfiguration<LOP>
    {
        public void Configure(EntityTypeBuilder<LOP> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("LOPId");
           
        }
    }
}
