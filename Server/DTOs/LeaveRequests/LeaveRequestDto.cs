using System.Collections;
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
    public string? AttachmentUrl { get; set; }
    public Status Status { get; set; }
    public string? FeedbackNotes { get; set; }

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
            AttachmentUrl = leaveRequestDto.AttachmentUrl,
            Status = leaveRequestDto.Status,
            FeedbackNotes = leaveRequestDto.FeedbackNotes,
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
            AttachmentUrl = leaveRequest.AttachmentUrl,
            Status = leaveRequest.Status,
            FeedbackNotes = leaveRequest.FeedbackNotes,
        };
    }
}