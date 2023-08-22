using Server.Utilities.Enums;

namespace Server.DTOs.LeaveRequests;

public class CreateLeaveRequestDto
{
    public Guid EmployeeGuid { get; set; }
    public LeaveType LeaveType { get; set; }
    public DateTime LeaveStart { get; set; }
    public DateTime LeaveEnd { get; set; }
    public string? Notes { get; set; }
    public byte[]? Attachment { get; set; }
}