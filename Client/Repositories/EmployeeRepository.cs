using Client.Contracts;
using Client.Repositories;
using Server.DTOs.Employees;
using Server.Models;
using System.Net.Http;

namespace Client.Repositories;

public class EmployeeRepository : GeneralRepository<Employee, Guid>, IEmployeeRepository
{
 
    public EmployeeRepository(string request = "employees/") : base(request)
    {
        
    }
}
