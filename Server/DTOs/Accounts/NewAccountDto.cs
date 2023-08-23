using Server.Models;

namespace Server.DTOs.Accounts;

public class NewAccountDto
{
    public string Password { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? ExpiredTime { get; set; }

    public static implicit operator Account(NewAccountDto accountDto)
    {
        return new Account
        {
            Guid = new Guid(),
            Password = accountDto.Password,
            Otp = 111111,
            IsUsed = accountDto.IsUsed,
            ExpiredTime = accountDto.ExpiredTime,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator NewAccountDto(Account account)
    {
        return new NewAccountDto
        {
            Password = account.Password,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        };
    }
}