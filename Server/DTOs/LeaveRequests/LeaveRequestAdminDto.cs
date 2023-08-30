using Server.Models;
using Server.Utilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Server.DTOs.LeaveRequests;

public class LeaveRequestAdminDto
{
    public Guid Guid { get; set; }
    [Display(Name = "Full Name")]
    public string FullName { get; set; }
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
    [Display(Name = "Feedback Notes")]
    public string? FeedbackNotes { get; set; }

   
}
