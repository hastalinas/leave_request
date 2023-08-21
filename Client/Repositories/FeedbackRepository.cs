using Client.Contracts;
using Server.Models;

namespace Client.Repositories;

public class FeedbackRepository : GeneralRepository<Feedback, Guid>, IFeedbackRepository
{
    public FeedbackRepository(string request = "feedbacks/") : base(request)
    {
    }
}
