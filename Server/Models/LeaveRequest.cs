using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Server.Utilities.Enums;

namespace Server.Models;

[Table("tb_m_leave_requests")]
public class LeaveRequest : BaseTable
{
    
    
    [Column("employee_guid")]
    [ForeignKey("Employee")]
    public Guid EmployeeGuid { get; set; }

    [Column("leave_type")]
    public LeaveType LeaveType { get; set; }

    [Column("leave_start")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LeaveStart { get; set; }

    [Column("leave_end")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LeaveEnd { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("attachment")]
    public string? AttachmentUrl { get; set; }

    public Status Status { get; set; }
    public string? FeedbackNotes { get; set; }
    
    public virtual Employee Employee { get; set; }
}