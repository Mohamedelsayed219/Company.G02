using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CompanyG02.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyG02.DAL.Data.Contexts
{
    public class CompanyDbContext : DbContext
    {

        public CompanyDbContext() : base ()
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server =.;Database = CompanyG02;Trusted_Connection = True ; TrustServerCertificate = Tru");
        }

        public DbSet<Department> Departments { get; set; }
        
    }
}
