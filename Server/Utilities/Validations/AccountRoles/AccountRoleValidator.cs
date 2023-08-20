﻿using FluentValidation;
using Server.Contracts;
using Server.DTOs.AccountRoles;

namespace Server.Utilities.Validations.AccountRoles;

public class AccountRoleValidator : AbstractValidator<AccountRoleDto>
{
    public readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleValidator(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
        RuleFor(dto => dto.Guid)
            .NotEmpty().WithMessage("Guid is required");
        RuleFor(dto => dto.AccountGuid)
            .NotEmpty().WithMessage("Account GUID is required");

        RuleFor(dto => dto.RoleGuid)
            .NotEmpty().WithMessage("Role GUID is required");
    }
}