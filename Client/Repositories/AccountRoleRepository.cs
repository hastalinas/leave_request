using Client.Contracts;
using Server.DTOs.AccountRoles;
using Server.Models;

namespace Client.Repositories;

public class AccountRoleRepository : GeneralRepository<AccountRoleDto, Guid>, IAccountRoleRepository
{
    public AccountRoleRepository(string request = "account-role/") : base(request)
    {

    }
}
