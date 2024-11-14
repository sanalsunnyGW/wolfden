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
        public WolfDenContext(DbContextOptions<WolfDenContext> options):base(options) { }       
            
        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Calling the AddConventions extension method
            modelBuilder.AddConventions("dbo", Assembly.GetExecutingAssembly());
        }
    }
   
}
