using Microsoft.EntityFrameworkCore;
using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class AccountRoleRepository : GeneralRepository<AccountRole>, IAccountRoleRepository
{
    public AccountRoleRepository(LeaveDbContext context) : base(context)
    {
    }

    public IEnumerable<string>? GetRoleNamesByAccountGuid(Guid guid)
    {
        var result = _context.Set<AccountRole>()
            .Where(ar => ar.AccountGuid == guid)
            .Include(ar => ar.Role)
            .Select(ar => ar.Role!.Name);

        return result;
    }

    public bool IsRoleDuplicate(Guid accountGuid, Guid roleGuid)
    {
        var accountRole = (
        from acc in _context.Accounts
        join accRole in _context.AccountRoles on acc.Guid equals accRole.AccountGuid
        join role in _context.Roles on accRole.RoleGuid equals role.Guid
        where acc.Guid == accountGuid && role.Guid == roleGuid
        select new
        {
            Guid = acc.Guid,
            Name = role.Guid,
        });
        if (accountRole is null)
        {
            return true;
        }
        return false;

    }
}