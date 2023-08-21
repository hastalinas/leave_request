using Server.Models;

namespace Client.Contracts;

public interface IFeedbackRepository : IRepository<Feedback, Guid>
{
}
