using FluentValidation;
using Server.DTOs.Departments;

namespace Server.Utilities.Validations.Departments
{
    public class NewDepartmentValidator : AbstractValidator<NewDepartmentDto>
    {
        public NewDepartmentValidator()
        {
            RuleFor(departmentDto => departmentDto.Name)
                .NotEmpty().WithMessage("Department name is required.");

            RuleFor(departmentDto => departmentDto.Code)
                .NotEmpty().WithMessage("Department code is required.");
        }
    }
}