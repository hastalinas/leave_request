using Server.Models;
using Server.Utilities.Enums;

namespace Server.DTOs.LeaveRequests;

public class LeaveRequestDetailDto
{
    public Guid Guid { get; set; }
    public string RequestNumber { get; set; }
    public string RelationManager { get; set; }
    public LeaveType LeaveType { get; set; }
    public DateTime LeaveStart { get; set; }
    public DateTime LeaveEnd { get; set; }
    public string PhoneNumber { get; set; }
    public string? Notes { get; set; }
    public TimeSpan LeaveDays { get; set; }
    public string Attachment { get; set; }
    public Status Status { get; set; }
}