using Server.Models;

namespace Server.Contracts;

public interface IAccountRoleRepository : IGeneralRepository<AccountRole>
{
    IEnumerable<string>? GetRoleNamesByAccountGuid(Guid guid);
}