using Client.Contracts;
using Newtonsoft.Json;
using Server.DTOs.Accounts;
using Server.Utilities.Handler;
using Client.Contracts;
using System.Text;
using Server.Models;

namespace Client.Repositories;

public class AccountRepository : GeneralRepository<Account, Guid>, IAccountRepository
{
    private readonly HttpClient httpClient;
    private readonly string request;
    public AccountRepository(string request = "accounts/") : base(request)
    {
        httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7293/api/")
        };
        this.request = request;
    }

    public async Task<ResponseHandler<TokenDto>> Login(LoginDto entity)
    {
        ResponseHandler<TokenDto> entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        using (var response = httpClient.PostAsync(request + "Login", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<TokenDto>>(apiResponse);
        }
        return entityVM;
    }

}
