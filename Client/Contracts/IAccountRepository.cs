using Server.DTOs.Accounts;
using Server.Utilities.Handler;
using Server.Models;
using NuGet.Protocol.Plugins;

namespace Client.Contracts;

public interface IAccountRepository : IRepository<Account, Guid>
{
    public Task<ResponseHandler<TokenDto>> Login(LoginDto entity);
    public Task<ResponseHandler<RegisterDto>> Register(RegisterDto entity);
    
}
