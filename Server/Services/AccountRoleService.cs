using Server.Contracts;
using Server.DTOs.AccountRoles;
using Server.Models;

namespace Server.Services;

public class AccountRoleService
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleService(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
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
        enumerable.ToList().ForEach( accountRole => accountRoleDtos.Add((AccountRoleDto)accountRole));

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
}