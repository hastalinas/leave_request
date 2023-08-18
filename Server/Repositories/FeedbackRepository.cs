using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class FeedbackRepository : GeneralRepository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(LeaveDbContext context) : base(context)
    {
    }
}