using Server.DTOs.Departments;
using Server.Models;
using Server.Utilities.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Server.DTOs.Employees;

public class EmployeeDto
{
    public Guid Guid { get; set; }
    public string Nik { get; set; }
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

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

    [Display(Name = "Manager")]
    public Guid? ManagerGuid { get; set; }

    [Display(Name = "Leave Remain")]
    public int LeaveRemain { get; set; }
    public DateTime LastLeaveUpdate { get; set; }

    public static implicit operator Employee(EmployeeDto employeeDto)
    {
        return new Employee
        {
            Guid = employeeDto.Guid,
            Nik = employeeDto.Nik,
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            BirthDate = employeeDto.BirthDate,
            Gender = employeeDto.Gender,
            HiringDate = employeeDto.HiringDate,
            Email = employeeDto.Email,
            PhoneNumber = employeeDto.PhoneNumber,
            DepartmentGuid = employeeDto.DepartmentGuid,
            ManagerGuid = employeeDto.ManagerGuid,
            LastLeaveUpdate = employeeDto.LastLeaveUpdate,
            LeaveRemain = employeeDto.LeaveRemain,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator EmployeeDto(Employee employee)
    {
        return new EmployeeDto
        {
            Guid = employee.Guid,
            Nik = employee.Nik,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            DepartmentGuid = employee.DepartmentGuid,
            ManagerGuid = employee.ManagerGuid,
            LeaveRemain = employee.LeaveRemain,
            LastLeaveUpdate = employee.LastLeaveUpdate
        };
    }
}