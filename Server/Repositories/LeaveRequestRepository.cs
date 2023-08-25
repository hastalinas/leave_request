using System.IdentityModel.Tokens.Jwt;
using Server.Contracts;
using Server.Data;
using Server.DTOs.LeaveRequests;
using Server.Models;
using Server.Utilities.Handler;

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
