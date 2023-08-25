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
using Server.Utilities.Enums;
using Server.Utilities.Handler;
using Server.DTOs.AccountRoles;

namespace Client.Controllers;

[Authorize(Roles = "employee, manager, admin")]
public class LeaveRequestController : Controller
{
    private readonly ILeaveRequestRepository _repository;
    public LeaveRequestController(ILeaveRequestRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _repository.Get();
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
            AttachmentUrl = leaveRequest.Attachment
        };

        var result = await _repository.Post(register);

        if (result.Code == 200)
        {
            RedirectToAction("Index");
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _repository.Get(id);
        var listRequest = new LeaveRequestDto();

        if (result.Data != null)
        {
            listRequest = (LeaveRequestDto)result.Data;
        }
        return View(listRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Update(LeaveRequestDto leave)
    {
        var result = await _repository.Put(leave.Guid, leave);

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
        var result = await _repository.Delete(guid);

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

    [HttpPost]
    public async Task<IActionResult> Send(Guid guid)
    {
        var leaveRequest = await _repository.Get(guid);
        leaveRequest.Data.Status = Status.OnProcess;
        var result = await _repository.Put(guid, leaveRequest.Data);
        if (result.Code == 200)
        {
            TempData["Success"] = $"{result.Message}!";
        }
        else
        {
            TempData["Error"] = $"{result.Message}!";
        }

        return RedirectToAction("Index", "LeaveRequest");
    }

    [HttpGet]
    public async Task<IActionResult> Account()
    {
        var result = await _repository.GetInfo();
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


    
