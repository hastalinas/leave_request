namespace Server.DTOs.Accounts;

public class OtpResponseDto
{
    public string Email { get; set; }
    public Guid Guid { get; set; }
    public int OTP { get; set; }
}