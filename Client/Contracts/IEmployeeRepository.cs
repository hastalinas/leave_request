using Server.DTOs.Employees;
using Server.Models;

namespace Client.Contracts;

public interface IEmployeeRepository : IRepository<EmployeeDto, Guid>
{
}
