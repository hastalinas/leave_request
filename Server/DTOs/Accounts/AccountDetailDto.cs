using Server.Models;
using Server.Utilities.Enums;

namespace Server.DTOs.Accounts;

public class AccountDetailDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
}