using Client.Contracts;
using Client.Repositories;
using Newtonsoft.Json;
using Server.DTOs.Employees;
using Server.Models;
using Server.Utilities.Handler;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Client.Repositories;

public class EmployeeRepository : GeneralRepository<EmployeeDto, Guid>, IEmployeeRepository
{
    protected readonly string _request;
    protected readonly HttpClient _httpClient;
    protected readonly IHttpContextAccessor _contextAccessor;
    public EmployeeRepository(string request = "employees/") : base(request)
    {
        this._request = request;
        _contextAccessor = new HttpContextAccessor();
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7293/api/")
        };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _contextAccessor.HttpContext?.Session.GetString("JWToken"));

    }
    public async Task<ResponseHandler<IEnumerable<EmployeeWithName>>?> GetAllEmployeewithName()
    {
        ResponseHandler<IEnumerable<EmployeeWithName>>? entityVM = null;
        using (var response = await _httpClient.GetAsync(_request + "employee-with-name"))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<IEnumerable<EmployeeWithName>>>(apiResponse);
        }
        return entityVM;
    }
}
