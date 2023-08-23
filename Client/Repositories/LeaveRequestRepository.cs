using System.Security.Claims;
using Client.Contracts;
using Server.DTOs.Employees;
using Server.DTOs.LeaveRequests;
using Server.Models;
using Server.Utilities.Handler;

namespace Client.Repositories;

public class LeaveRequestRepository : GeneralRepository<LeaveRequestDto, Guid>, ILeaveRequestRepository
{
    public LeaveRequestRepository(string request = "leave-request/") : base(request)
    {

    }
}
