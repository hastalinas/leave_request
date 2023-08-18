using Server.Contracts;
using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class RoleRepository : GeneralRepository<Role>, IRoleRepository
{
    public RoleRepository(LeaveDbContext context) : base(context)
    {
    }
}