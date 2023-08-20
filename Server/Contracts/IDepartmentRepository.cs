using Server.Models;

namespace Server.Contracts;

public interface IDepartmentRepository : IGeneralRepository<Department>
{
    Department? GetByCode(string code);
}