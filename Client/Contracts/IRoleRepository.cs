using Server.Models;

namespace Client.Contracts;

public interface IRoleRepository : IRepository<Role, Guid>
{
}
