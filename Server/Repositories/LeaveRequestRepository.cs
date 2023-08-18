using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class LeaveRequestRepository : GeneralRepository<LeaveRequest>, ILeaveRequestRepository
{
    public LeaveRequestRepository(LeaveDbContext context) : base(context)
    {
    }
}