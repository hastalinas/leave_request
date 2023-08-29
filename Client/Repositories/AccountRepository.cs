using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Client.Contracts;
using Newtonsoft.Json;
using Server.DTOs.Accounts;
using Server.Utilities.Handler;
using Client.Contracts;
using System.Text;
using Microsoft.Net.Http.Headers;
using Server.Models;
using NuGet.Protocol.Plugins;
using Server.DTOs.LeaveRequests;
using ContentDispositionHeaderValue = System.Net.Http.Headers.ContentDispositionHeaderValue;

namespace Client.Repositories;

public class AccountRepository : GeneralRepository<AccountDto, Guid>, IAccountRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _request;
    private readonly IWebHostEnvironment _env;

    public AccountRepository(IWebHostEnvironment env,
        string request = "accounts/") : base(request)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7293/api/")
        };
        this._request = request;
        _env = env;
    }

    public async Task<ResponseHandler<TokenDto>?> Login(LoginDto entity)
    {
        ResponseHandler<TokenDto>? entityVM = null;
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request + "login", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<TokenDto>>(apiResponse);
        }

        return entityVM;
    }


    public async Task<ResponseHandler<RegisterDto>?> Register(RegisterDto entity)
    {
        entity.PhoneNumber = "+62" + entity.PhoneNumber;
        ResponseHandler<RegisterDto>? entityVM = null;
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
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
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
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
        StringContent content =
            new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        using (var response = await _httpClient.PostAsync(_request + "change-password", content))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<ChangePasswordDto>>(apiResponse);
        }

        return entityVM;
    }

    public async Task<ResponseHandler<IEnumerable<AccountDetailDto>>?> GetDetailAll()
    {
        ResponseHandler<IEnumerable<AccountDetailDto>>? entityVM = null;
        using (var response = await _httpClient.GetAsync(_request + "detail"))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<IEnumerable<AccountDetailDto>>>(apiResponse);
        }

        return entityVM;
    }
    
    public async Task<bool> UploadAvatar(Guid accountId, IFormFile avatarFile)
    {
        if (avatarFile != null && avatarFile.Length > 0)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, "avatars");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(stream);
            }

            var apiUrl = _request + accountId + "/upload-avatar"; // Sesuaikan dengan endpoint API Anda
            var fileStreamContent = new StreamContent(new FileStream(filePath, FileMode.Open));
            fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "avatar",
                FileName = fileName
            };

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(fileStreamContent);

            var response = await _httpClient.PostAsync(apiUrl, multipartContent);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }

        return false;
    }
}
