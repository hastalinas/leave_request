using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Utilities.Enums;

namespace Server.Models;

[Table("tb_m_feedback")]
public class Feedback : BaseTable
{
    [Column("leave_request")]
    [ForeignKey("LeaveRequest")]
    public Guid LeaveRequestGuid { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }
    
    public virtual LeaveRequest LeaveRequest { get; set; }
}