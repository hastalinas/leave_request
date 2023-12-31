﻿using Server.DTOs.AccountRoles;
using Server.DTOs.Employees;
using Server.DTOs.LeaveRequests;
using Server.Models;
using Server.Utilities.Handler;

namespace Client.Contracts;

public interface ILeaveRequestRepository : IRepository<LeaveRequestDto, Guid>
{
    Task<ResponseHandler<IEnumerable<LeaveRequestDetailDto>>> GetInfo();
    Task<ResponseHandler<IEnumerable<LeaveRequestDetailDto>>> GetInfoManager();
    Task<ResponseHandler<RegisterLeaveDto>> RegisterLeave(RegisterLeaveDto entity);
    Task<ResponseHandler<IEnumerable<LeaveRequestAdminDto>>> GetLeaveRequestAdmin();

}
