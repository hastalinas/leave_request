using FluentValidation;
using Server.DTOs.Accounts;

namespace Server.Utilities.Validations.Accounts
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(forgotPassword => forgotPassword.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}