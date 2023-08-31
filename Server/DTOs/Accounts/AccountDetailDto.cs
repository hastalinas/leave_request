using Server.Models;
using Server.Utilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Server.DTOs.Accounts;

public class AccountDetailDto
{
    public Guid Guid { get; set; }
    public string Nik { get; set; }
    public string Name { get; set; }
    [Display(Name = "Birth Date")]
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string Email { get; set; }
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
}