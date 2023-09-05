using System.Globalization;
using Server.Utilities.Enums;
using Server.DTOs.LeaveRequests;

namespace Client.Models
{
    public class EditLeaveRequestDto
    {
        public Guid Guid { get; set; }
        public Guid EmployeeGuid { get; set; }
        public LeaveType LeaveType { get; set; }
        
        public string RequestNumber { get; set; }
        public string LeaveStart { get; set; }
        public string LeaveEnd { get; set; }
        public string? Notes { get; set; }
        public string? Attachment { get; set; }
        public Status Status { get; set; }
        public IFormFile? FileUpload { get; set; }
        public string? FeedbackNotes { get; set; }

        // Operator implisit untuk mengonversi dari RegisterLeaveRequestDto ke RegisterLeaveDto
        public static implicit operator LeaveRequestDto(EditLeaveRequestDto source)
        {
            if (source == null)
                return null;

            return new LeaveRequestDto
            {
                LeaveType = source.LeaveType,
                RequestNumber = source.RequestNumber,
                LeaveStart = DateTime.ParseExact(source.LeaveStart, "dd-MM-yyyy", CultureInfo.InvariantCulture), // Mengubah format tanggal
                LeaveEnd = DateTime.ParseExact(source.LeaveEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture), // Mengubah format tanggal
                Notes = source.Notes,
                AttachmentUrl = source.Attachment,
                Status = source.Status,
                Guid = source.Guid,
                EmployeeGuid = source.EmployeeGuid,
                FeedbackNotes = source.FeedbackNotes
            };
        }

        // Operator eksplisit untuk mengonversi dari RegisterLeaveDto ke RegisterLeaveRequestDto
        public static explicit operator EditLeaveRequestDto(LeaveRequestDto source)
        {
            if (source == null)
                return null;

            return new EditLeaveRequestDto
            {
                LeaveType = source.LeaveType,
                RequestNumber = source.RequestNumber,
                LeaveStart = source.LeaveStart.ToString("dd-MM-yyyy"),
                LeaveEnd = source.LeaveEnd.ToString("dd-MM-yyyy"),
                Notes = source.Notes,
                Attachment = source.AttachmentUrl,
                Status = source.Status,
                Guid = source.Guid,
                EmployeeGuid = source.EmployeeGuid,
                FeedbackNotes = source.FeedbackNotes
            };
        }

        // Metode untuk mengubah format tanggal
        private static string ConvertDateFormat(string inputDate, string sourceFormat, string targetFormat)
        {
            if (string.IsNullOrEmpty(inputDate))
                return inputDate;

            DateTime date = DateTime.ParseExact(inputDate, sourceFormat, null);
            return date.ToString(targetFormat);
        }
    }
}
