using Server.Models;
using Server.Utilities.Enums;

namespace Server.DTOs.LeaveRequests;

public class NewLeaveRequestDto
{
    public Guid EmployeeGuid { get; set; }
    public LeaveType LeaveType { get; set; }
    public DateTime LeaveStart { get; set; }
    public DateTime LeaveEnd { get; set; }
    public string? Notes { get; set; }
    public string? Attachment { get; set; }
    public Status Status { get; set; }

    public static implicit operator LeaveRequest(NewLeaveRequestDto newLeaveRequestDto)
    {
        return new LeaveRequest
        {
            Guid = new Guid(),
            EmployeeGuid = newLeaveRequestDto.EmployeeGuid,
            LeaveType = newLeaveRequestDto.LeaveType,
            LeaveStart = newLeaveRequestDto.LeaveStart,
            LeaveEnd = newLeaveRequestDto.LeaveEnd,
            Notes = newLeaveRequestDto.Notes,
            AttachmentUrl = newLeaveRequestDto.Attachment,
            Status = newLeaveRequestDto.Status,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator NewLeaveRequestDto(LeaveRequest leaveRequest)
    {
        return new NewLeaveRequestDto
        {
            EmployeeGuid = leaveRequest.EmployeeGuid,
            LeaveType = leaveRequest.LeaveType,
            LeaveStart = leaveRequest.LeaveStart,
            LeaveEnd = leaveRequest.LeaveEnd,
            Notes = leaveRequest.Notes,
            Attachment = leaveRequest.AttachmentUrl,
            Status = leaveRequest.Status
        };
    }
}