using Server.DTOs.Accounts;
using Server.Utilities.Handler;
using Server.Models;

namespace Client.Contracts;

public interface IAccountRepository : IRepository<Account, Guid>
{
    public Task<ResponseHandler<TokenDto>> Login(LoginDto entity);
}
