using Server.Models;

namespace Server.DTOs.Accounts;

public class AccountDto
{
    public Guid Guid { get; set; }
    public string? ProfilPictureUrl { get; set; }
    public string Password { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? ExpiredTime { get; set; }

    public static implicit operator Account(AccountDto accountDto)
    {
        return new Account
        {
            Guid = accountDto.Guid,
            ProfilPictureUrl = accountDto.ProfilPictureUrl,
            Password = accountDto.Password,
            Otp = 111111,
            IsUsed = accountDto.IsUsed,
            ExpiredTime = accountDto.ExpiredTime,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static explicit operator AccountDto(Account account)
    {
        return new AccountDto
        {
            Guid = account.Guid,
            ProfilPictureUrl = account.ProfilPictureUrl,
            Password = account.Password,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        };
    }
}