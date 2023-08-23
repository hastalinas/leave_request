using Client.Contracts;
using Server.DTOs.Feedbacks;
using Server.Models;

namespace Client.Repositories;

public class FeedbackRepository : GeneralRepository<FeedbackDto, Guid>, IFeedbackRepository
{
    public FeedbackRepository(string request = "feedbacks/") : base(request)
    {
    }
}
