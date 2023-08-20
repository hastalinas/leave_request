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
                new Role { Guid = Guid.Parse("36350d33-42d7-4c63-a244-29b0a8d13bce"), Name = "admin", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Role { Guid = Guid.Parse("4887ec13-b482-47b3-9b24-08db91a71770"), Name = "employee", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Role { Guid = Guid.Parse("a7e15d29-9c74-4e72-ae63-5a47d69b27d6"), Name = "manager", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
            );

            // Other model configurations

            base.OnModelCreating(modelBuilder);
        }
    }
}