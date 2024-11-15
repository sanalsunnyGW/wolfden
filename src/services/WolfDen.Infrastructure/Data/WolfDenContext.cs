using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<LeaveDay> LeaveDays { get; set; }
        public DbSet<LeaveIncrementLog> LeaveIncrementLogs { get; set; }
        public DbSet<LeaveSetting> LeaveSettings { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }



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
