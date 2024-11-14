using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
       public DbSet<AttendenceClose> AttendenceClose { get; set; }
        public DbSet<AttendenceLog> AttendenceLog { get; set; }
       public DbSet<DailyAttendence> DailyAttendence { get; set; }
       public DbSet<Device> Device { get; set; }
       public DbSet<Holiday> Holiday { get; set; }
       public DbSet<Notification> Notification { get; set; }   
       public DbSet<Status> Status { get; set; }
        public DbSet<StatusType> StatusType { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new DesignationConfiguration());
            modelBuilder.ApplyConfiguration(new AttendenceCloseConfiguration());
            modelBuilder.ApplyConfiguration(new AttendenceLogConfiguration());
            modelBuilder.ApplyConfiguration(new DailyAttendenceConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new HolidayConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());

        }
    }
}
