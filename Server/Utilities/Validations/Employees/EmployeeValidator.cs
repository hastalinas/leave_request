using FluentValidation;
using Server.DTOs.Employees;

namespace Server.Utilities.Validations.Employees
{
    public class EmployeeValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeValidator()
        {
            RuleFor(dto => dto.Nik).NotEmpty().WithMessage("Nik harus diisi.");
            RuleFor(dto => dto.FirstName).NotEmpty().WithMessage("Nama depan harus diisi.");
            RuleFor(dto => dto.BirthDate).Must(BeAValidDate).WithMessage("Tanggal lahir tidak valid.")
                .Must(BeAtLeast18YearsOld).WithMessage("Usia harus minimal 18 tahun.");
            RuleFor(dto => dto.Email).NotEmpty().EmailAddress().WithMessage("Email harus diisi dan berupa alamat email yang valid.");
            RuleFor(dto => dto.PhoneNumber).NotEmpty().WithMessage("Nomor telepon harus diisi.");
            RuleFor(dto => dto.DepartmentGuid).NotEmpty().WithMessage("Guid departemen harus diisi.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date != default(DateTime);
        }

        private bool BeAtLeast18YearsOld(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
            {
                age--;
            }

            return age >= 18;
        }
    }
}