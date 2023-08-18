using Server.Data;
using Server.Models;

namespace Server.Repositories;

public class RoleRepository : GeneralRepository<Role>
{
    protected RoleRepository(LeaveDbContext context) : base(context)
    {
    }
}