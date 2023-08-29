using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Client.Contracts;
using Newtonsoft.Json;
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
    }
    
    public async Task<ResponseHandler<IEnumerable<LeaveRequestDetailDto>>> GetInfo()
    {
        var jwtToken = _contextAccessor.HttpContext?.Session.GetString("JWToken");
        var entityVM = new ResponseHandler<IEnumerable<LeaveRequestDetailDto>>();

        if (!string.IsNullOrEmpty(jwtToken))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ReadJwtToken(jwtToken).Claims;

            var guid = claims.FirstOrDefault(c => c.Type == "Guid")?.Value;
            
            using (var response = await _httpClient.GetAsync(_request + "request"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<IEnumerable<LeaveRequestDetailDto>>>(apiResponse);
            }
        }
        return entityVM;
    }
    
    public async Task<ResponseHandler<IEnumerable<LeaveRequestDetailDto>>> GetInfoManager()
    {
        var jwtToken = _contextAccessor.HttpContext?.Session.GetString("JWToken");
        var entityVM = new ResponseHandler<IEnumerable<LeaveRequestDetailDto>>();

        if (!string.IsNullOrEmpty(jwtToken))
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = tokenHandler.ReadJwtToken(jwtToken).Claims;

            var guid = claims.FirstOrDefault(c => c.Type == "Guid")?.Value;
            
            using (var response = await _httpClient.GetAsync(_request + "request-manager"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<IEnumerable<LeaveRequestDetailDto>>>(apiResponse);
            }
        }
        return entityVM;
    }

    public async Task<ResponseHandler<RegisterLeaveDto>> RegisterLeave(RegisterLeaveDto entity)
    {
        ResponseHandler<RegisterLeaveDto>? entityVM = null;
        StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        using (var response = _httpClient.PostAsync(_request + "request", content).Result)
        {
            string apiResponse = await response.Content.ReadAsStringAsync();
            entityVM = JsonConvert.DeserializeObject<ResponseHandler<RegisterLeaveDto>>(apiResponse);
        }
        return entityVM;
    }
}



