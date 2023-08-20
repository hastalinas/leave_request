using Server.Models;
using Server.Utilities.Enums;

namespace Server.DTOs.Feedbacks;

public class NewFeedbackDto
{
    public Guid LeaveRequestGuid { get; set; }
    public FeedbackStatus Status { get; set; }
    public string? Notes { get; set; }
    
    public static implicit operator Feedback(NewFeedbackDto feedbackDto)
    {
        return new Feedback
        {
            Guid = new Guid(),
            LeaveRequestGuid = feedbackDto.LeaveRequestGuid,
            Status = feedbackDto.Status,
            Notes = feedbackDto.Notes,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator NewFeedbackDto(Feedback feedback)
    {
        return new NewFeedbackDto
        {
            LeaveRequestGuid = feedback.LeaveRequestGuid,
            Status = feedback.Status,
            Notes = feedback.Notes,
        };
    }
}