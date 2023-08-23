using Client.Contracts;
using Newtonsoft.Json;
using Server.DTOs.Accounts;
using Server.Utilities.Handler;
using Client.Contracts;
using System.Text;
using Server.Models;
using NuGet.Protocol.Plugins;

namespace Client.Repositories;

public class AccountRepository : GeneralRepository<AccountDto, Guid>, IAccountRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _request;
    public AccountRepository(string request = "accounts/") : base(request)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7293/api/")
        };
        this._request = request;
    }

    public async Task<ResponseHandler<TokenDto>?> Login(LoginDto entity)
    {
        ResponseHandler<TokenDto>? entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request + "login", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<TokenDto>>(apiResponse);
        }
        return entityVM;
    }


    public async Task<ResponseHandler<RegisterDto>?> Register(RegisterDto entity)
    {
        ResponseHandler<RegisterDto>? entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request + "register", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<RegisterDto>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandler<ForgotPasswordDto>?> ForgotPassword(ForgotPasswordDto entity)
    {
        ResponseHandler<ForgotPasswordDto>? entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        using (var response = await _httpClient.PostAsync(_request + "forgot-password", content))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<ForgotPasswordDto>>(apiResponse);
        }
        return entityVM;
    }

    public async Task<ResponseHandler<ChangePasswordDto>?> ChangePassword(ChangePasswordDto entity)
    {
        ResponseHandler<ChangePasswordDto>? entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        using (var response = await _httpClient.PostAsync(_request + "change-password", content))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<ChangePasswordDto>>(apiResponse);
        }
        return entityVM;
    }
}
