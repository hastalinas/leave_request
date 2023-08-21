using Server.Models;

namespace Client.Contracts;

public interface ILeaveRequestRepository : IRepository<LeaveRequest, Guid>
{
}
