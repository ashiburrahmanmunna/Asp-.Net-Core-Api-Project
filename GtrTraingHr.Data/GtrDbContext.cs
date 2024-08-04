using GtrTraingHr.Models;
using Microsoft.EntityFrameworkCore;

namespace GtrTraingHr.Data
{
    public class GtrDbContext : DbContext
    {
        public GtrDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AttendanceSummary> AttendanceSummaries { get; set; }
        public DbSet<Salary> Salaries { get; set; }
    }
}
