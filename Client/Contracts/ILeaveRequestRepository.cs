using Server.DTOs.LeaveRequests;
using Server.Models;

namespace Client.Contracts;

public interface ILeaveRequestRepository : IRepository<LeaveRequestDto, Guid>
{
}
