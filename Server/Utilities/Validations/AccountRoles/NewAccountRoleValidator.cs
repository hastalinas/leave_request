using FluentValidation;
using Server.Contracts;
using Server.DTOs.AccountRoles;

namespace Server.Utilities.Validations.AccountRoles;

public class NewAccountRoleDtoValidator : AbstractValidator<NewAccountRoleDto>
{
    public readonly IAccountRoleRepository _accountRoleRepository;

    public NewAccountRoleDtoValidator(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
        RuleFor(dto => dto.AccountGuid)
            .NotEmpty().WithMessage("Account GUID is required");

        RuleFor(dto => dto.RoleGuid)
            .NotEmpty().WithMessage("Role GUID is required");
    }
}