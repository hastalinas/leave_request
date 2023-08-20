using Server.Models;
using Server.Utilities.Enums;

namespace Server.DTOs.LeaveRequests;

public class LeaveRequestDto
{
    public Guid Guid { get; set; }
    public Guid EmployeeGuid { get; set; }
    public LeaveType LeaveType { get; set; }
    public DateTime LeaveStart { get; set; }
    public DateTime LeaveEnd { get; set; }
    public string? Notes { get; set; }
    public byte[]? Attachment { get; set; }

    public static implicit operator LeaveRequest(LeaveRequestDto leaveRequestDto)
    {
        return new LeaveRequest
        {
            Guid = leaveRequestDto.Guid,
            EmployeeGuid = leaveRequestDto.EmployeeGuid,
            LeaveType = leaveRequestDto.LeaveType,
            LeaveStart = leaveRequestDto.LeaveStart,
            LeaveEnd = leaveRequestDto.LeaveEnd,
            Notes = leaveRequestDto.Notes,
            Attachment = leaveRequestDto.Attachment,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator LeaveRequestDto(LeaveRequest leaveRequest)
    {
        return new LeaveRequestDto
        {
            Guid = leaveRequest.Guid,
            EmployeeGuid = leaveRequest.EmployeeGuid,
            LeaveType = leaveRequest.LeaveType,
            LeaveStart = leaveRequest.LeaveStart,
            LeaveEnd = leaveRequest.LeaveEnd,
            Notes = leaveRequest.Notes,
            Attachment = leaveRequest.Attachment
        };
    }
}