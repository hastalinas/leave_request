using FluentValidation;
using Server.Contracts;
using Server.DTOs.Employees;
using Server.DTOs.LeaveRequests;

namespace Server.Utilities.Validations.LeaveRequests
{
    public class RegisterLeaveValidator : AbstractValidator<RegisterLeaveDto>
    {
        public readonly ILeaveRequestRepository _leaveRequestRepository;
        public RegisterLeaveValidator(ILeaveRequestRepository leaveRequestRepository)
        {
            _leaveRequestRepository = leaveRequestRepository;

            RuleFor(dto => dto.LeaveType).IsInEnum().WithMessage("Tipe Cuti tidak valid.");
            RuleFor(dto => dto.LeaveStart).NotEmpty().WithMessage("Tanggal Mulai Cuti harus diisi.");
            RuleFor(dto => dto.LeaveEnd).NotEmpty().WithMessage("Tanggal Selesai Cuti harus diisi.")
                .GreaterThanOrEqualTo(dto => dto.LeaveStart).WithMessage("Tanggal Selesai Cuti harus setelah Tanggal Mulai Cuti.");
            RuleFor(dto => dto.Notes).MaximumLength(200).WithMessage("Catatan tidak boleh lebih dari 200 karakter.");
            // RuleFor(dto => dto.Attachment).NotEmpty().WithMessage("Lampiran harus diisi.");
            RuleFor(dto => dto.Status);
        }
    }
}