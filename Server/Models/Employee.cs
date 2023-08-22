using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Utilities.Enums;

namespace Server.Models;

[Table("tb_m_employees")]
public class Employee : BaseTable
{
    [Column("nik")]
    public string Nik { get; set; }
    
    [Column("first_name")]
    public string FirstName { get; set; }
    
    [Column("last_name")]
    public string? LastName { get; set; }
    
    [Column("birth_date")]
    public DateTime BirthDate { get; set; }
    
    [Column("gender")]
    public Gender Gender { get; set; }
    
    [Column("hiring_date")]
    public DateTime HiringDate { get; set; }
    
    [Column("email")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [Index(IsUnique = true)]
    public string Email { get; set; }
    
    [Column("phone_number")]
    [Index(IsUnique = true)]
    public string PhoneNumber { get; set; }
    
    [Column("department_guid")]
    [ForeignKey("Department")]
    public Guid DepartmentGuid { get; set; }
    public virtual Department Department { get; set; }
    
    [Column("manager_guid")]
    [ForeignKey("Manager")]
    public Guid? ManagerGuid { get; set; }
    
    [Column("leave_remain")]
    public int LeaveRemain { get; set; }
    public DateTime LastLeaveUpdate { get; set; }

    public virtual Employee Manager { get; set; }
    public virtual Account Account { get; set; }
    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
}
