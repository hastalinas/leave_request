using Server.Contracts;
using Server.DTOs.LeaveRequests;
using Server.Models;

namespace Server.Services;

public class LeaveRequestService
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;

    public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository)
    {
        _leaveRequestRepository = leaveRequestRepository;
    }
    
    public IEnumerable<LeaveRequestDto> GetAll()
    {
        var leaveRequests = _leaveRequestRepository.GetAll();
        var enumerable = leaveRequests.ToList();
        if (!enumerable.Any())
        {
            return Enumerable.Empty<LeaveRequestDto>();
        }

        var leaveRequestDtos = new List<LeaveRequestDto>();
        enumerable.ToList().ForEach(feedback => leaveRequestDtos.Add((LeaveRequestDto)feedback));

        return leaveRequestDtos;
    }

    public LeaveRequestDto? GetByGuid(Guid guid)
    {
        var leaveRequest = _leaveRequestRepository.GetByGuid(guid);
        if (leaveRequest is null)
        {
            return null;
        }

        return (LeaveRequestDto)leaveRequest;
    }

    public NewLeaveRequestDto? Create(NewLeaveRequestDto newLeaveRequestDto)
    {
        LeaveRequest lr = newLeaveRequestDto;

        var leaveRequest = _leaveRequestRepository.Create(lr);
        if (leaveRequest is null)
        {
            return null;
        }

        return (NewLeaveRequestDto)leaveRequest;
    }

    public int Update(LeaveRequestDto leaveRequestDto)
    {
        var role = _leaveRequestRepository.GetByGuid(leaveRequestDto.Guid);
        if (role is null)
        {
            return -1;
        }

        LeaveRequest toUpdate = leaveRequestDto;
        toUpdate.CreatedDate = role.CreatedDate;
        var result = _leaveRequestRepository.Update(toUpdate);

        return result ? 1 : 0;
    }

    public int Delete(Guid guid)
    {
        var leaveRequest = _leaveRequestRepository.GetByGuid(guid);
        if (leaveRequest is null)
        {
            return -1;
        }

        var result = _leaveRequestRepository.Delete(leaveRequest);
        return result ? 1 : 0;
    }
}