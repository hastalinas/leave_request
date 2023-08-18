using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class LeaveRequestRepository : GeneralRepository<LeaveRequest>
{
    protected LeaveRequestRepository(LeaveDbContext context) : base(context)
    {
    }
}