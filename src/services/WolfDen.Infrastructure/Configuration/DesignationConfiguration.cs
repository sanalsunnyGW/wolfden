using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class DesignationConfiguration : IEntityTypeConfiguration<Designation>
    {
        public void Configure(EntityTypeBuilder<Designation> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(256);
            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}
