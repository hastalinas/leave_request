using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class DepartmentRepository : GeneralRepository<Department>
{
    protected DepartmentRepository(LeaveDbContext context) : base(context)
    {
    }
}