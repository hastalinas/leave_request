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
            
            // Department seeding
            modelBuilder.Entity<Department>().HasData(
                new Department { Guid = Guid.Parse("b41e4d54-2ffe-4619-53c8-08dba0d4ed05"), Name = "Sales", Code = "SALES", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Department { Guid = Guid.Parse("51d55a47-1cab-42e6-53c9-08dba0d4ed05"), Name = "Marketing", Code = "MARKETING", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Department { Guid = Guid.Parse("1fcc1546-78e3-4baf-53ca-08dba0d4ed05"), Name = "Finance", Code = "FINANCE", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Department { Guid = Guid.Parse("e707fb58-cdb1-4c2a-53cb-08dba0d4ed05"), Name = "Human Resources", Code = "HR", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Department { Guid = Guid.Parse("02988287-198d-4dab-53cc-08dba0d4ed05"), Name = "Research and Development", Code = "RND", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Department { Guid = Guid.Parse("8ccd5722-3f93-484d-53cd-08dba0d4ed05"), Name = "Information Technology", Code = "IT", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Department { Guid = Guid.Parse("9b3c7c65-c99a-4e97-53ce-08dba0d4ed05"), Name = "Operations", Code = "OPS", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Department { Guid = Guid.Parse("1e4f0537-3ca0-4d64-53cf-08dba0d4ed05"), Name = "Customer Service", Code = "CS", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Department { Guid = Guid.Parse("bb4e21b9-f8ac-40ad-53d0-08dba0d4ed05"), Name = "Production", Code = "PROD", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
                new Department { Guid = Guid.Parse("5eac3979-fc26-4017-53d1-08dba0d4ed05"), Name = "Quality Assurance", Code = "QA", CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
            );

           

            // Other model configurations

            base.OnModelCreating(modelBuilder);
        }
    }
}