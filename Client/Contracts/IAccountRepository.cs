using Server.DTOs.Accounts;
using Server.Utilities.Handler;
using Server.Models;
using NuGet.Protocol.Plugins;

namespace Client.Contracts;

public interface IAccountRepository : IRepository<AccountDto, Guid>
{
    Task<ResponseHandler<TokenDto>?> Login(LoginDto entity);
    Task<ResponseHandler<RegisterDto>?> Register(RegisterDto entity);
    Task<ResponseHandler<ForgotPasswordDto>?> ForgotPassword(ForgotPasswordDto entity);
    Task<ResponseHandler<ChangePasswordDto>?> ChangePassword(ChangePasswordDto entity);
    Task<ResponseHandler<IEnumerable<AccountDetailDto>>?> GetDetailAll();

}
