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
    public DateTime LeaveStart { get; set; }

    [Column("leave_end")]
    public DateTime LeaveEnd { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("attachment")]
    public byte[] Attachment { get; set; }
    
    public virtual Employee Employee { get; set; }
    public virtual Feedback Feedback { get; set; }
}