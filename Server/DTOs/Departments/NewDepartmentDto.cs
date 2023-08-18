using Server.Models;

namespace Server.DTOs.Departments;

public class NewDepartmentDto
{
    public string Name { get; set; }
    public string Code { get; set; }

    public static implicit operator Department(NewDepartmentDto newDepartmentDto)
    {
        return new Department
        {
            Guid = new Guid(),
            Name = newDepartmentDto.Name,
            Code = newDepartmentDto.Code,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator NewDepartmentDto(Department account)
    {
        return new NewDepartmentDto
        {
            Name = account.Name,
            Code = account.Code
        };
    }
}