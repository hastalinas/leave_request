using Server.DTOs.AccountRoles;
using Server.Models;
using Server.Utilities.Handler;

namespace Client.Contracts;

public interface IAccountRoleRepository : IRepository<AccountRole, Guid>
{
    public Task<ResponseHandler<IEnumerable<AccountRoleInfoDto>>> accountRoleInfo();
}
