using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class LeaveRequestRepository : GeneralRepository<LeaveRequest>, ILeaveRequestRepository
{
    public LeaveRequestRepository(LeaveDbContext context) : base(context)
    {
    }

    public int CountStatus0(Guid guid)
    {
        return _context.Set<LeaveRequest>()
            .Count(lr => lr.EmployeeGuid == guid && lr.Status == 0);
    }
}
