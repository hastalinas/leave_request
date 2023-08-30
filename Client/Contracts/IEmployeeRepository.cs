using Server.DTOs.Employees;
using Server.Models;
using Server.Utilities.Handler;

namespace Client.Contracts;

public interface IEmployeeRepository : IRepository<EmployeeDto, Guid>
{
    public Task<ResponseHandler<IEnumerable<EmployeeWithNameDto>>?> GetAllEmployeewithName();
}
