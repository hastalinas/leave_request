using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data
{
    public class LeaveDbContext : DbContext
    {
        public LeaveDbContext(DbContextOptions<LeaveDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Role> Roles { get; set; } // Add DbSet for Role
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Role seeding
            modelBuilder.Entity<Role>().HasData(
                new Role { Guid = Guid.NewGuid(), Name = "admin", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Role { Guid = Guid.NewGuid(), Name = "employee", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Role { Guid = Guid.NewGuid(), Name = "manager", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
            );

            // Other model configurations

            base.OnModelCreating(modelBuilder);
        }
    }
}