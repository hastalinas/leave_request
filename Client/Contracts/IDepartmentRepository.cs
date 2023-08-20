using Server.Models;

namespace Client.Contracts;

public interface IDepartmentRepository : IRepository<Department, Guid>
{
}
