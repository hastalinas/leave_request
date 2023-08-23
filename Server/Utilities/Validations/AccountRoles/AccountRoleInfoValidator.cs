using FluentValidation;
using Server.Contracts;
using Server.DTOs.AccountRoles;

namespace Server.Utilities.Validations.AccountRoles;

public class AccountRoleInfoValidator : AbstractValidator<AccountRoleInfoDto>
{
    public readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleInfoValidator(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
        RuleFor(dto => dto.Guid)
            .NotEmpty().WithMessage("Guid is required");
        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Account GUID is required");
           

        RuleFor(dto => dto.Role)
            .NotEmpty().WithMessage("Role GUID is required");
    }

  
}
