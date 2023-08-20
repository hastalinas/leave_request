using Server.Models;

namespace Server.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    bool IsNotExist(string value);
    string? GetLastNik();
    Employee? GetByEmail(string email);
    Employee? CheckEmail(string email);
    Guid GetLastEmployeeGuid();
    Employee? GetByNik(string nik);
}