using FluentValidation;
using Server.DTOs.Feedbacks;

namespace Server.Utilities.Validations.Feedbacks
{
    public class NewFeedbackValidator : AbstractValidator<NewFeedbackDto>
    {
        public NewFeedbackValidator()
        {
            RuleFor(dto => dto.LeaveRequestGuid).NotEmpty().WithMessage("Guid Leave Request harus diisi.");
            RuleFor(dto => dto.Status).IsInEnum().WithMessage("Status tidak valid.");
            RuleFor(dto => dto.Notes).MaximumLength(200).WithMessage("Catatan tidak boleh lebih dari 200 karakter.");
        }
    }
}