using Server.Contracts;
using Server.DTOs.AccountRoles;
using Server.Models;

namespace Server.Services;

public class AccountRoleService
{
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IAccountRepository _accountRepository;

    public AccountRoleService(
        IAccountRoleRepository accountRoleRepository,
        IEmployeeRepository employeeRepository,
        IRoleRepository roleRepository,
        IAccountRepository accountRepository)
    {
        _accountRoleRepository = accountRoleRepository;
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _accountRepository = accountRepository;
    }

    public IEnumerable<AccountRoleDto> GetAll()
    {
        var accountRoles = _accountRoleRepository.GetAll();
        var enumerable = accountRoles.ToList();
        if (!enumerable.Any())
        {
            return Enumerable.Empty<AccountRoleDto>();
        }

        var accountRoleDtos = new List<AccountRoleDto>();
        enumerable.ToList().ForEach(accountRole => accountRoleDtos.Add((AccountRoleDto)accountRole));

        return accountRoleDtos;
    }

    public AccountRoleDto? GetByGuid(Guid guid)
    {
        var accountRole = _accountRoleRepository.GetByGuid(guid);
        if (accountRole is null)
        {
            return null;
        }

        return (AccountRoleDto)accountRole;
    }

    public NewAccountRoleDto? Create(NewAccountRoleDto newAccountRoleDto)
    {
        var accountRole = _accountRoleRepository.Create(newAccountRoleDto);
        if (accountRole is null)
        {
            return null;
        }

        return (NewAccountRoleDto)accountRole;
    }

    public int Update(AccountRoleDto accountRoleDto)
    {
        var accountRole = _accountRoleRepository.GetByGuid(accountRoleDto.Guid);
        if (accountRole is null)
        {
            return -1;
        }

        AccountRole toUpdate = accountRoleDto;
        toUpdate.CreatedDate = accountRole.CreatedDate;
        var result = _accountRoleRepository.Update(toUpdate);

        return result ? 1 : 0;
    }

    public int Delete(Guid guid)
    {
        var accountRole = _accountRoleRepository.GetByGuid(guid);
        if (accountRole is null)
        {
            return -1;
        }

        var result = _accountRoleRepository.Delete(accountRole);
        return result ? 1 : 0;
    }

    public IEnumerable<AccountRoleInfoDto> AccountRoleInfo()
    {
        var accountRoleInfoList = (
    from emp in _employeeRepository.GetAll()
    join acc in _accountRepository.GetAll() on emp.Guid equals acc.Guid
    join accRole in _accountRoleRepository.GetAll() on acc.Guid equals accRole.AccountGuid
    join role in _roleRepository.GetAll() on accRole.RoleGuid equals role.Guid
    select new 
    {
        Guid = emp.Guid,
        Nik = emp.Nik,
        Name = $"{emp.FirstName} {emp.LastName}",
        Role = role.Name
    }
).ToList();

var groupedData = accountRoleInfoList
    .GroupBy(info => new { info.Guid, info.Nik })
    .Select(group => new AccountRoleInfoDto
    {
        Guid = group.Key.Guid,
        Nik = group.Key.Nik,
        Name = group.First().Name,
        Role = group.Select(info => info.Role).ToList()
    })
    .ToList();

    return groupedData;
    }
}
