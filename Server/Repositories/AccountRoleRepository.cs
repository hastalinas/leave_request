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
        var exixtingData = _context.Set<AccountRole>()
            .FirstOrDefault(accRole => accRole.AccountGuid == accountGuid && accRole.RoleGuid == roleGuid);
        if (exixtingData is null)
        {
            return true;
        }

        return false;
    }

    public IEnumerable<AccountRole>? GetManager()
    {
        return _context.Set<AccountRole>().Where(ar => ar.RoleGuid == Guid.Parse("a7e15d29-9c74-4e72-ae63-5a47d69b27d6")).ToList();
    }
}