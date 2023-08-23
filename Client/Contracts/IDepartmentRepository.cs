using Server.DTOs.Departments;
using Server.Models;

namespace Client.Contracts;

public interface IDepartmentRepository : IRepository<DepartmentDto, Guid>
{
}
