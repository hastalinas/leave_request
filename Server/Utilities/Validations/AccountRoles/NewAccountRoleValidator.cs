using FluentValidation;
using Server.Contracts;
using Server.DTOs.AccountRoles;
using Server.Repositories;

namespace Server.Utilities.Validations.AccountRoles;

public class NewAccountRoleDtoValidator : AbstractValidator<NewAccountRoleDto>
{
    public readonly IAccountRoleRepository _accountRoleRepository;

    public NewAccountRoleDtoValidator(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
        RuleFor(dto => dto.AccountGuid)
            .NotEmpty().WithMessage("Account GUID is required")
            .Must((dto, roleGuid) => IsDuplicate(dto.AccountGuid, roleGuid));
    
        RuleFor(dto => dto.RoleGuid)
            .NotEmpty().WithMessage("Role GUID is required")
            .Must((dto, roleGuid) => IsDuplicate(dto.AccountGuid, roleGuid));
    }
    
    public bool IsDuplicate(Guid accountGuid, Guid roleGuid)
    {
        return _accountRoleRepository.IsRoleDuplicate(accountGuid, roleGuid);
    }
}