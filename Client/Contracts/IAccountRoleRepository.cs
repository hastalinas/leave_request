using Server.DTOs.AccountRoles;
using Server.Models;

namespace Client.Contracts;

public interface IAccountRoleRepository : IRepository<AccountRoleDto, Guid>
{
}
