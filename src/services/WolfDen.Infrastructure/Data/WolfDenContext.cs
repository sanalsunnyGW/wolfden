using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WolfDen.Domain.Entity;
using WolfDen.Infrastructure.Configuration;
using WolfDen.Infrastructure.Extensions;

namespace WolfDen.Infrastructure.Data
{
    public class WolfDenContext : DbContext
    {
        public WolfDenContext(DbContextOptions<WolfDenContext> options) : base(options) { }


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
        public DbSet<AttendanceStatus> AttendanceStatus { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Calling the AddConventions extension method
            modelBuilder.AddConventions("wolfden", Assembly.GetExecutingAssembly());


            //History Table

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.GetDefaultTableName());
                var historyTableName = $"{entityType.GetDefaultTableName()}";
                entityType.SetHistoryTableName(historyTableName);
                entityType.SetHistoryTableSchema("wolfdenHT");

                entityType.SetIsTemporal(true);

                foreach (var fk in entityType.GetForeignKeys().Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade))
                {
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
                }

            }

        }

    }

}
