using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WolfDen.Domain.Entity;

namespace WolfDen.Infrastructure.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {

        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();
            builder.HasIndex(x => x.EmployeeCode).IsUnique();
            builder.HasIndex(x => x.RFId).IsUnique();
            builder.Property(x => x.Id).HasMaxLength(256);
            builder.Property(x => x.FirstName).HasMaxLength(256);
            builder.Property(x => x.LastName).HasMaxLength(256);
            builder.Property(x => x.PhoneNumber).HasMaxLength(15);
            builder.Property(x => x.RFId).HasMaxLength(100);
            builder.HasOne(x => x.Designation).WithMany().HasForeignKey(x => x.DesignationId);
            builder.HasOne(x => x.Department).WithMany().HasForeignKey(x => x.DepartmentId);
            builder.HasOne(x => x.Manager).WithMany().HasForeignKey(x => x.ManagerId);
        }
    }
}
