using Client.Contracts;
using Server.Models;

namespace Client.Repositories;

public class LeaveRequestRepository : GeneralRepository<LeaveRequest, Guid>, ILeaveRequestRepository
{
    public LeaveRequestRepository(string request = "leave-request/") : base(request)
    {

    }
}
