namespace Server.DTOs.AccountRoles;

public class AccountRoleInfoDto
{
    public Guid Guid { set; get; }
    public string? Nik { get; set; }
    public string? Name { get; set; }
    public List<string> Role { get; set; }
}