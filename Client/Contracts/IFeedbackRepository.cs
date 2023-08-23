using Server.DTOs.Feedbacks;
using Server.Models;

namespace Client.Contracts;

public interface IFeedbackRepository : IRepository<FeedbackDto, Guid>
{
}
