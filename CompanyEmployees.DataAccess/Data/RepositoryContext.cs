using CompanyEmployees.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CompanyEmployees.DataAccess.Data
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyData());
            modelBuilder.ApplyConfiguration(new EmployeeData());
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
