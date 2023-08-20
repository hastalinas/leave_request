using Client.Contracts;
using Client.Repositories;
using Server.DTOs.Employees;
using Server.Models;

namespace Client.Repositories;

public class EmployeeRepository : GeneralRepository<Employee, Guid>, IEmployeeRepository
{
    public EmployeeRepository(string request = "employees/") : base(request)
    {
    }
}
