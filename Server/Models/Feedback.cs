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

    [Column("feedback_status")]
    public FeedbackStatus FeedbackStatus { get; set; }

    [Column("feedback_notes")]
    public string? FeedbackNotes { get; set; }
    
    public virtual LeaveRequest LeaveRequest { get; set; }
}