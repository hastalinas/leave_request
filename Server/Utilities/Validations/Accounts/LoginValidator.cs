using FluentValidation;
using Server.DTOs.Accounts;

namespace Server.Utilities.Validations.Accounts
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(login => login.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(login => login.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}