using Server.DTOs.AccountRoles;
using Server.DTOs.LeaveRequests;
using Server.Models;
using Server.Utilities.Handler;

namespace Client.Contracts;

public interface ILeaveRequestRepository : IRepository<LeaveRequestDto, Guid>
{
    public Task<ResponseHandler<IEnumerable<RequestInformationDto>>> Detail();
    public Task<ResponseHandler<IEnumerable<LeaveRequestDetailDto>>> Employee();
}
