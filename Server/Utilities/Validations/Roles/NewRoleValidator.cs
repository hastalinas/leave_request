using FluentValidation;
using Server.DTOs.Roles;

namespace Server.Utilities.Validations.Roles
{
    public class NewRoleValidator : AbstractValidator<NewRoleDto>
    {
        public NewRoleValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty().WithMessage("Nama peran harus diisi.")
                .MaximumLength(50).WithMessage("Nama peran tidak boleh lebih dari 50 karakter.");
        }
    }
}