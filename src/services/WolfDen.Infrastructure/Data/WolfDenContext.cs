using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Configuration;

namespace WolfDen.Infrastructure.Data
{
    public class WolfDenContext:DbContext
    {
        public WolfDenContext(DbContextOptions<WolfDenContext> options):base(options) { }
        
            
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<LeaveRequestDay> LeaveDays { get; set; }
        public DbSet<LeaveIncrementLog> LeaveIncrementLogs { get; set; }
        public DbSet<LeaveSetting> LeaveSettings { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new DesignationConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveBalanceConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveRequestConfiguration());   
            modelBuilder.ApplyConfiguration(new LeaveSettingConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveTypeConfiguration());  
            modelBuilder.ApplyConfiguration(new LeaveDayConfiguration());
            modelBuilder.ApplyConfiguration(new LeaveIncrementLogConfiguration());
        }
    }
}
