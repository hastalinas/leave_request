using Client.Contracts;
using Server.Models;

namespace Client.Repositories;

public class AccountRoleRepository : GeneralRepository<AccountRole, Guid>, IAccountRoleRepository
{
    public AccountRoleRepository(string request = "accountroles/") : base(request)
    {

    }
}
