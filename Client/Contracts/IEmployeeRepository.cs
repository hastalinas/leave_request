using Server.Models;

namespace Client.Contracts;

public interface IEmployeeRepository : IRepository<Employee, Guid>
{
}
