using Server.Models;
using Client.Contracts;

namespace Client.Repositories;

public class DepartmentRepository : GeneralRepository<Department, Guid>, IDepartmentRepository
{
    public DepartmentRepository(string request = "departments/") : base(request)
    {

    }
}
