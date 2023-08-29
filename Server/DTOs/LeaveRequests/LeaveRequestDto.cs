using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Server.Models;
using Server.Utilities.Enums;

namespace Server.DTOs.LeaveRequests;

public class LeaveRequestDto
{
    public Guid Guid { get; set; }
    public Guid EmployeeGuid { get; set; }
    [Display(Name = "Leave Type")]
    public LeaveType LeaveType { get; set; }
    [Display(Name = "Leave Start")]
    public DateTime LeaveStart { get; set; }
    [Display(Name = "Leave End")]
    public DateTime LeaveEnd { get; set; }
    public string? Notes { get; set; }
    [Display(Name = "Attachment Url")]
    public string? AttachmentUrl { get; set; }
    public Status Status { get; set; }

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
            Status = leaveRequest.Status
        };
    }

    public IEnumerator GetEnumerator()
    {
        yield break;
    }
}