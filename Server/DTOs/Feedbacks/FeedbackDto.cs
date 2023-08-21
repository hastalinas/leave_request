using Server.Models;
using Server.Utilities.Enums;

namespace Server.DTOs.Feedbacks;

public class FeedbackDto
{
    public Guid Guid { get; set; }
    public Guid LeaveRequestGuid { get; set; }
    public string? Notes { get; set; }
    
    public static implicit operator Feedback(FeedbackDto feedbackDto)
    {
        return new Feedback
        {
            Guid = feedbackDto.Guid,
            LeaveRequestGuid = feedbackDto.LeaveRequestGuid,
            Notes = feedbackDto.Notes,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator FeedbackDto(Feedback feedback)
    {
        return new FeedbackDto
        {
            Guid = feedback.Guid,
            LeaveRequestGuid = feedback.LeaveRequestGuid,
            Notes = feedback.Notes,
        };
    }
}