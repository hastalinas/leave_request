using Server.Models;

namespace Server.Contracts;

public interface ILeaveRequestRepository : IGeneralRepository<LeaveRequest>
{
    int CountStatus0(Guid guid);
}