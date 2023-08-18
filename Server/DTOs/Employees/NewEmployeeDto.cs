using Server.DTOs.Departments;
using Server.Models;
using Server.Utilities.Enums;

namespace Server.DTOs.Employees;

public class NewEmployeeDto
{
    public string Nik { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public DateTime HiringDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Guid DepartmentGuid { get; set; }
    public Guid ManagerGuid { get; set; }
    
    public static implicit operator Employee(NewEmployeeDto employeeDto)
    {
        return new Employee
        {
            Guid = new Guid(),
            Nik = employeeDto.Nik,
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            BirthDate = employeeDto.BirthDate,
            Gender = employeeDto.Gender,
            HiringDate = employeeDto.HiringDate,
            Email = employeeDto.Email,
            PhoneNumber = employeeDto.PhoneNumber,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator NewEmployeeDto(Employee employee)
    {
        return new NewEmployeeDto
        {
            Nik = employee.Nik,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            HiringDate = employee.HiringDate,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
        };
    }
}