using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Accounts;
using Server.DTOs.LeaveRequests;
using Server.Utilities.Enums;

namespace Client.Controllers;

public class EmployeeLeaveReportController : Controller
{
    public readonly ILeaveRequestRepository _leaveRequestRepository;

    public EmployeeLeaveReportController(ILeaveRequestRepository leaveRequestRepository)
    {
        _leaveRequestRepository = leaveRequestRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> Account()
    {
        var result = await _leaveRequestRepository.GetInfo();
        var listAccount = new List<LeaveRequestDetailDto>();
        try
        {
            listAccount = result.Data.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return View(listAccount);
    }

    public async Task<IActionResult> Notification()
    {
        
        return View();
    }
}