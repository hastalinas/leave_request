using Server.Utilities.Enums;

namespace Client.Models;

public class AccountUploadDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }

    // New property for image upload
    public IFormFile? ProfileImage { get; set; }
}