using System.Globalization;
using Server.Utilities.Enums;
using Server.DTOs.LeaveRequests;

namespace Client.Models
{
    public class RegisterLeaveRequestDto
    {
        public LeaveType LeaveType { get; set; }
        public string LeaveStart { get; set; }
        public string LeaveEnd { get; set; }
        public string? Notes { get; set; }
        public string? Attachment { get; set; }
        public Status Status { get; set; }
        public IFormFile? FileUpload { get; set; }

        // Operator implisit untuk mengonversi dari RegisterLeaveRequestDto ke RegisterLeaveDto
        public static implicit operator RegisterLeaveDto(RegisterLeaveRequestDto source)
        {
            if (source == null)
                return null;

            return new RegisterLeaveDto
            {
                LeaveType = source.LeaveType,
                LeaveStart = DateTime.ParseExact(source.LeaveStart, "dd-MM-yyyy", CultureInfo.InvariantCulture), // Mengubah format tanggal
                LeaveEnd = DateTime.ParseExact(source.LeaveEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture), // Mengubah format tanggal
                Notes = source.Notes,
                Attachment = source.Attachment,
                Status = source.Status
            };
        }

        // Operator eksplisit untuk mengonversi dari RegisterLeaveDto ke RegisterLeaveRequestDto
        public static explicit operator RegisterLeaveRequestDto(RegisterLeaveDto source)
        {
            if (source == null)
                return null;

            return new RegisterLeaveRequestDto
            {
                LeaveType = source.LeaveType,
                LeaveStart = ConvertDateFormat(source.LeaveStart.ToString(), "MM/dd/yyyy", "dd/MM/yyyy"), // Mengubah format tanggal
                LeaveEnd = ConvertDateFormat(source.LeaveEnd.ToString(), "MM/dd/yyyy", "dd/MM/yyyy"), // Mengubah format tanggal
                Notes = source.Notes,
                Attachment = source.Attachment,
                Status = source.Status
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
