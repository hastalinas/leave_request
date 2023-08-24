using System.Net.Http;
using System.Security.Claims;
using Client.Contracts;
using Newtonsoft.Json;
using Server.DTOs.AccountRoles;
using Server.DTOs.Employees;
using Server.DTOs.LeaveRequests;
using Server.Models;
using Server.Utilities.Handler;

namespace Client.Repositories;

public class LeaveRequestRepository : GeneralRepository<LeaveRequestDto, Guid>, ILeaveRequestRepository
{
    private readonly HttpClient httpClient;
    private readonly string request;
    public LeaveRequestRepository(string request = "leave-request/") : base(request)
    {
        httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7293/api/")
        };
        this.request = request;

    }
    public async Task<ResponseHandler<IEnumerable<RequestInformationDto>>> Detail()
    {
        ResponseHandler<IEnumerable<RequestInformationDto>> entityVM = null;
        using (var response = await httpClient.GetAsync(request + "detail"))
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<IEnumerable<RequestInformationDto>>>(apiResponse);
        }
        return entityVM;

    }
}



