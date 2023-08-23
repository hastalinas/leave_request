using Server.Utilities.Enums;

namespace Server.DTOs.Employees;

public class RegisterLeaveDto
{
    public LeaveType LeaveType { get; set; }
    public DateTime LeaveStart { get; set; }
    public DateTime LeaveEnd { get; set; }
    public string? Notes { get; set; }
    public string? Attachment { get; set; }
    public Status Status { get; set; }
}