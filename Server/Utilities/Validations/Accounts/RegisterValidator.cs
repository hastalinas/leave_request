using FluentValidation;
using Server.DTOs.Accounts;

namespace Server.Utilities.Validations.Accounts
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(register => register.FirstName)
                .NotEmpty().WithMessage("First name is required.");

            RuleFor(register => register.BirthDate)
                .NotEmpty().WithMessage("Birth date is required.");

            RuleFor(register => register.Gender)
                .IsInEnum().WithMessage("Invalid gender value.");

            RuleFor(register => register.HiringDate)
                .NotEmpty().WithMessage("Hiring date is required.");

            RuleFor(register => register.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(register => register.PhoneNumber)
                .MaximumLength(15).WithMessage("Phone number is invalid.")
                .NotEmpty().WithMessage("Phone number is required.");

            RuleFor(register => register.DepartmentCode)
                .NotEmpty().WithMessage("Department code is required.");

            RuleFor(register => register.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]").WithMessage("Password must contain at least one special character.");

            RuleFor(register => register.ConfirmPassword)
                .Equal(register => register.Password).WithMessage("Passwords do not match.");
        }
    }
}