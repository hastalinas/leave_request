using Server.Models;
using Client.Contracts;
using Server.DTOs.Departments;

namespace Client.Repositories;

public class DepartmentRepository : GeneralRepository<DepartmentDto, Guid>, IDepartmentRepository
{
    public DepartmentRepository(string request = "departments/") : base(request)
    {

    }
}
