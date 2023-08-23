using Client.Contracts;
using Server.DTOs.Roles;

namespace Client.Repositories;

public class RoleRepository : GeneralRepository<RoleDto, Guid>, IRoleRepository
{
    public RoleRepository(string request = "roles/") : base(request)
    {
    }
}
