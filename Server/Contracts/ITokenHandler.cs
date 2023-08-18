using System.Security.Claims;

namespace Server.Contracts;

public interface ITokenHandler
{
    string? GenerateToken(IEnumerable<Claim> claims);
}