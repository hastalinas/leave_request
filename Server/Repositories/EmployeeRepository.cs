using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(LeaveDbContext context) : base(context)
    {
    }

    public bool IsNotExist(string value)
    {
        return _context.Set<Employee>()
            .SingleOrDefault(e => e.Email.Contains(value)
                                  || e.PhoneNumber.Contains(value)) is null;
    }

    public string? GetLastNik()
    {
        return _context.Set<Employee>().OrderBy(e => e.Nik)
            .LastOrDefault()
            ?.Nik;
    }

    public Employee? GetByEmail(string email)
    {
        return _context.Set<Employee>().SingleOrDefault(e => e.Email.Contains(email));
    }

    public Employee? CheckEmail(string email)
    {
        return _context.Set<Employee>().FirstOrDefault(u => u.Email == email);
    }

    public Guid GetLastEmployeeGuid()
    {
        return _context.Set<Employee>().LastOrDefault()!.Guid;
    }
    
    public Employee? GetByNik(string nik)
    {
        return _context.Set<Employee>().SingleOrDefault(e => e.Nik.Contains(nik));
    }

    public IEnumerable<Employee>? GetManager()
    {
        return _context.Set<Employee>().ToList().Where(employee => employee.ManagerGuid != null);
    }
}