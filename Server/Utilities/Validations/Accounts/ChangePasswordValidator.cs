using FluentValidation;
using Server.DTOs.Accounts;

namespace Server.Utilities.Validations.Accounts
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(changePassword => changePassword.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(changePassword => changePassword.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(8).WithMessage("New password must be at least 8 characters.")
                .Matches("[0-9]").WithMessage("New password must contain at least one digit.")
                .Matches("[A-Z]").WithMessage("New password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("New password must contain at least one lowercase letter.")
                .Matches("[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]").WithMessage("New password must contain at least one special character.");

            RuleFor(changePassword => changePassword.ConfirmPassword)
                .Equal(changePassword => changePassword.NewPassword).WithMessage("New password and confirm password do not match.");

            RuleFor(changePassword => changePassword.OTP)
                .NotEmpty().WithMessage("OTP is required.");
        }
    }
}