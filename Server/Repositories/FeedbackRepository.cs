using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class FeedbackRepository : GeneralRepository<Feedback>
{
    protected FeedbackRepository(LeaveDbContext context) : base(context)
    {
    }
}