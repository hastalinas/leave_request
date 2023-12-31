﻿using Server.Models;
using Server.Utilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Server.DTOs.Employees;

public class EmployeeWithNameDto
{
    public Guid Guid { get; set; }
    public string Nik { get; set; }
    [Display(Name = "Full Name")]
    public string FullName { get; set; }

    [Display(Name = "Birth Date")]
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    [Display(Name = "Hiring Date")]
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    [Display(Name = "Department")]
    public Guid DepartmentGuid { get; set; }
    [Display(Name = "Department")]
    public string DepartmentName { get; set; }

    public Guid? ManagerGuid { get; set; }
    [Display(Name = "Manager")]
    public string? ManagerName { get; set; }

    [Display(Name = "Leave Remain")]
    public int LeaveRemain { get; set; }
    public DateTime LastLeaveUpdate { get; set; }

    public IFormFile? ProfileImage { get; set; }
    public string profilImageUrl { get; set; }
}
