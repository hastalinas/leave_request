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

public class LeaveRequestController : Controller
{
    private readonly ILeaveRequestRepository _repository;
    private readonly IEmployeeRepository _employeeRepository;
    public LeaveRequestController(ILeaveRequestRepository repository, IEmployeeRepository employeeRepository)
    {
        _repository = repository;
        _employeeRepository = employeeRepository;
    }


    [Authorize(Roles = "manager, admin, employee")]
    public async Task<IActionResult> Index()
    {
        var result = await _repository.GetInfoManager();
        var ListRequest = new List<LeaveRequestDetailDto>();

        try
        {
            ListRequest = result.Data.ToList();
        }
        catch
        {
            // ignored
        }

        return View(ListRequest);
    }

    [HttpGet]
    [Authorize(Roles = "employee")]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RegisterLeaveDto leaveRequest)
    {
        var emp = await _employeeRepository.Get();
        
        // Mendapatkan klaim-klaim dari pengguna yang terautentikasi
        var userClaims = User.Claims;

        var enumerable = userClaims.ToList();
        var guid = enumerable.FirstOrDefault(c => c.Type == "Guid")?.Value;
        var data = emp.Data.FirstOrDefault(e => e.Guid == Guid.Parse(guid));

        if (data.LeaveRemain >= (leaveRequest.LeaveEnd - leaveRequest.LeaveStart).Days)
        {
            var register = new RegisterLeaveDto()
            {
                LeaveType = leaveRequest.LeaveType,
                LeaveStart = leaveRequest.LeaveStart,
                LeaveEnd = leaveRequest.LeaveEnd,
                Notes = leaveRequest.Notes
            };

            var result = await _repository.RegisterLeave(register);

            if (result.Code == 200)
            {
                RedirectToAction("Index");
            }
        }
        else
        {
            TempData["Error"] = $"Leave remain not enough! Your leave remain : {data.LeaveRemain}";
        }
        return View();
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
    [Authorize(Roles = "manager, admin, employee")]
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
    [Authorize(Roles = "manager, admin, employee")]
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

        return RedirectToAction("Account", "LeaveRequest");
    }

    [HttpPost]
    [Authorize(Roles = "employee")]
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

        return RedirectToAction("Account", "LeaveRequest");
    }

    [HttpGet]
    [Authorize(Roles = "employee")]
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

    [HttpGet]
    public async Task<IActionResult> EditEmployee(Guid id)
    {
        var result = await _repository.Get(id);
        var listRequest = new LeaveRequestDto();

        if (result.Data != null)
        {
            listRequest = (LeaveRequestDto)result.Data;
        }
        return View(listRequest);
    }

    [HttpGet]
    public async Task<IActionResult> Response(Guid id)
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
    public async Task<IActionResult> Response(LeaveRequestDto entity)
    {
        var result = await _repository.Put(entity.Guid, entity);

        var employee = await _employeeRepository.Get();

        try
        {
            if (entity.Status == Status.Success && result.Data != null)
            {
                var data = employee.Data.FirstOrDefault(e => e.Guid == entity.EmployeeGuid);
                data.LeaveRemain = data.LeaveRemain - (entity.LeaveEnd - entity.LeaveStart).Days;

                var udate = await _employeeRepository.Put(data.Guid, data);
            }
        }
        catch
        {
            // ignored
        }

        return RedirectToAction("Index", "LeaveRequest");
    }

    public async Task<IActionResult> Notification()
    {

        return View();
    }

  

}


    
