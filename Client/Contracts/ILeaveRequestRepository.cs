using Server.DTOs.LeaveRequests;
using Server.Models;
using Server.Utilities.Handler;

namespace Client.Contracts;

public interface ILeaveRequestRepository : IRepository<LeaveRequestDto, Guid>
{
    Task<ResponseHandler<IEnumerable<LeaveRequestDetailDto>>> GetInfo();
}
