using FluentValidation;
using Server.Contracts;
using Server.DTOs.Accounts;

namespace Server.Utilities.Validations.Accounts;

public class NewAccountValidator: AbstractValidator<NewAccountDto>
{
    private readonly IAccountRepository _accountRepository;

    public NewAccountValidator(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
        RuleFor(a => a.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]").WithMessage("Password must contain at least one special character.");
        RuleFor(a => a.ExpiredTime)
            .Must(BeValidExpiredTime).WithMessage("Expired time must be within 2 hours from now.");
    }
    
    private bool BeValidExpiredTime(DateTime? expiredTime)
    {
        if (!expiredTime.HasValue)
        {
            return false;
        }

        DateTime currentUtc = DateTime.UtcNow;
        DateTime maxExpiredTime = currentUtc.AddHours(2);

        return expiredTime.Value <= maxExpiredTime;
    }
}