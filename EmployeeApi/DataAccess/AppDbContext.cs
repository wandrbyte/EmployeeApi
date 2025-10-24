using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
    }
}
