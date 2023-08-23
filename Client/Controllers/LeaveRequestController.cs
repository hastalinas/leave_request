using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Departments;
using Server.DTOs.LeaveRequests;
using Server.Models;
using System.Data;
using System.Security.Claims;
using Server.DTOs.Employees;
using Server.Utilities.Handler;

namespace Client.Controllers;

[Authorize(Roles = "employee, anager, admin")]
public class LeaveRequestController : Controller
{
    private readonly ILeaveRequestRepository repository;
    public LeaveRequestController(ILeaveRequestRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListRequest = new List<LeaveRequestDto>();

        if (result.Data != null)
        {
            ListRequest = result.Data.ToList();
        }
        return View(ListRequest);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RegisterLeaveDto leaveRequest)
    {
        // Mendapatkan klaim-klaim dari pengguna yang terautentikasi
        var userClaims = User.Claims;

        var enumerable = userClaims.ToList();
        var guidClaim = enumerable.FirstOrDefault(c => c.Type == "Guid")?.Value;
    
        // Dapatkan peran (role) pengguna dari klaim tipe "Role"
        var roles = enumerable.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

        var register = new LeaveRequestDto()
        {
            Guid = new Guid(),
            EmployeeGuid = Guid.Parse(guidClaim),
            LeaveType = leaveRequest.LeaveType,
            LeaveStart = leaveRequest.LeaveStart,
            LeaveEnd = leaveRequest.LeaveEnd,
            Notes = leaveRequest.Notes,
            Attachment = leaveRequest.Attachment
        };
    
        var result = await repository.Post(register);

        if (result.Code == 200)
        {
            RedirectToAction("Index");
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await repository.Get(id);
        var ListRequest = new LeaveRequestDto();

        if (result.Data != null)
        {
            ListRequest = (LeaveRequestDto)result.Data;
        }
        return View(ListRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Update(LeaveRequestDto leave)
    {
        var result = await repository.Put(leave.Guid, leave);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Updated! - {result.Message}!";
            return RedirectToAction("Index", "LeaveRequest");
        }
        return RedirectToAction(nameof(Edit));
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var result = await repository.Delete(guid);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Deleted! - {result.Message}!";
        }
        else
        {
            TempData["Error"] = $"Failed to Delete Data - {result.Message}!";
        }

        return RedirectToAction("Index", "LeaveRequest");
    }
}
