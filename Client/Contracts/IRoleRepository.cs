using Server.DTOs.Roles;
using Server.Models;

namespace Client.Contracts;

public interface IRoleRepository : IRepository<RoleDto, Guid>
{
}
