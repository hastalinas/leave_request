using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class DepartmentRepository : GeneralRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(LeaveDbContext context) : base(context)
    {
    }
    
    public Department? GetByCode(string code)
    {
        
        return _context.Set<Department>().SingleOrDefault(u => u.Code.Contains(code));
    }
}