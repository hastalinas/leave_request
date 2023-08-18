
using Server.Models;

namespace Server.DTOs.Roles;

public class RoleDto
{
    public Guid Guid { get; set; } 
    public string Name { get; set; }

    public static implicit operator Role(RoleDto roleDto)
    {
        return new Role()
        {
            Guid = roleDto.Guid,
            Name = roleDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator RoleDto(Role role)
    {
        return new RoleDto()
        {
            Guid = role.Guid,
            Name = role.Name
        };
    }
}