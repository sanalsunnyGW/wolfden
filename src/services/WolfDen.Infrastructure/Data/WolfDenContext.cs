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
    public interface IDummyEntity
    {
    }
    public class WolfDenContext : DbContext
    {
        public WolfDenContext(DbContextOptions<WolfDenContext> options):base(options) { }       
            
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Calling the AddConventions extension method
            modelBuilder.AddConventions("dbo", Assembly.GetExecutingAssembly());


            //History Table

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.GetDefaultTableName());

                if (!entityType.ClrType.IsAssignableTo(typeof(IDummyEntity)))
                {
                    var historyTableName = $"{entityType.GetDefaultTableName()}_HT"; 
                    entityType.SetHistoryTableName(historyTableName);
                    entityType.SetHistoryTableSchema("dbo"); 

                    entityType.SetIsTemporal(true);

                    foreach (var fk in entityType.GetForeignKeys().Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade))
                    {
                        fk.DeleteBehavior = DeleteBehavior.Restrict;
                    }
                }
            }
        
        }

    }
   
}
