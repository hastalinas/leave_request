using FluentValidation;
using Server.DTOs.LeaveRequests;

namespace Server.Utilities.Validations.LeaveRequests
{
    public class LeaveRequestValidator : AbstractValidator<LeaveRequestDto>
    {
        public LeaveRequestValidator()
        {
            RuleFor(dto => dto.EmployeeGuid).NotEmpty().WithMessage("Guid Pegawai harus diisi.");
            RuleFor(dto => dto.LeaveType).IsInEnum().WithMessage("Tipe Cuti tidak valid.");
            RuleFor(dto => dto.LeaveStart).NotEmpty().WithMessage("Tanggal Mulai Cuti harus diisi.");
            RuleFor(dto => dto.LeaveEnd).NotEmpty().WithMessage("Tanggal Selesai Cuti harus diisi.")
                .GreaterThanOrEqualTo(dto => dto.LeaveStart).WithMessage("Tanggal Selesai Cuti harus setelah Tanggal Mulai Cuti.");
            RuleFor(dto => dto.Notes).MaximumLength(200).WithMessage("Catatan tidak boleh lebih dari 200 karakter.");
            RuleFor(dto => dto.Attachment).NotEmpty().WithMessage("Lampiran harus diisi.");
        }
    }
}