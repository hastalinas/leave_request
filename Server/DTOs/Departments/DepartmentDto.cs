using Server.Models;

namespace Server.DTOs.Departments;

public class DepartmentDto
{
    public Guid Guid { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }

    public static implicit operator Department(DepartmentDto departmentDto)
    {
        return new Department
        {
            Guid = departmentDto.Guid,
            Name = departmentDto.Name,
            Code = departmentDto.Code,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator DepartmentDto(Department account)
    {
        return new DepartmentDto
        {
            Guid = account.Guid,
            Name = account.Name,
            Code = account.Code
        };
    }
}