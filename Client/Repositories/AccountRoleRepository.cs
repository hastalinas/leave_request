using Newtonsoft.Json;
using Server.DTOs.AccountRoles;
using Server.DTOs.Accounts;
using Server.Models;
using Server.Utilities.Handler;
using System.Net.Http;
using System.Text;
using Client.Contracts;

namespace Client.Repositories;

public class AccountRoleRepository : GeneralRepository<AccountRoleDto, Guid>, IAccountRoleRepository
{
    private readonly HttpClient httpClient;
    private readonly string request;
    public AccountRoleRepository(string request = "account-role/") : base(request)
    {
        httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7293/api/")
        };
        this.request = request;
    }

    public async Task<ResponseHandler<IEnumerable<AccountRoleInfoDto>>> accountRoleInfo()
    {
        ResponseHandler<IEnumerable<AccountRoleInfoDto>> entityVM = null;
        using (var response = await httpClient.GetAsync(request + "info"))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<IEnumerable<AccountRoleInfoDto>>>(apiResponse);
        }
        return entityVM;
    }


    //method baru delete account role - linq
    //public async Task<ResponseHandler<AccountRoleDto>> Delete
}
