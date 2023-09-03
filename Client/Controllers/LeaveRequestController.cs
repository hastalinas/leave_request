using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Departments;
using Server.DTOs.LeaveRequests;
using Server.Models;
using System.Data;
using System.Security.Claims;
using Client.Models;
using Microsoft.Net.Http.Headers;
using Server.DTOs.Employees;
using Server.Utilities.Enums;
using Server.Utilities.Handler;
using Server.DTOs.AccountRoles;

namespace Client.Controllers;

public class LeaveRequestController : Controller
{
    private readonly ILeaveRequestRepository _repository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IWebHostEnvironment _env;
    public LeaveRequestController(ILeaveRequestRepository repository, IEmployeeRepository employeeRepository, IWebHostEnvironment env)
    {
        _repository = repository;
        _employeeRepository = employeeRepository;
        _env = env;
    }


    [Authorize(Roles = "manager, admin")]
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

    public async Task<IActionResult> IndexAdmin()
    {
        var result = await _repository.GetLeaveRequestAdmin();
        var listRequest = new List<LeaveRequestAdminDto>();
        try
        {
            foreach (var employee in result.Data)
            {
                var LeaveRequestNewList = new LeaveRequestAdminDto
                {
                    Guid = employee.Guid,
                    EmployeeGuid = employee.EmployeeGuid,
                    FullName = employee.FullName,
                    LeaveType = employee.LeaveType,
                    LeaveStart = employee.LeaveStart,
                    LeaveEnd = employee.LeaveEnd,
                    Notes = employee.Notes,
                    AttachmentUrl = employee.AttachmentUrl,
                    Status = employee.Status,
                    FeedbackNotes = employee.FeedbackNotes
                };
                listRequest.Add(LeaveRequestNewList);
            }
        }
        catch
        {

        }

        if (result.Data != null)
        {
            listRequest = result.Data.ToList();
        }
        return View(listRequest);
    }


    [HttpGet]
    [Authorize(Roles = "employee")]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RegisterLeaveRequestDto leaveRequest)
    {
        var userClaims = User.Claims;
        var guid = userClaims.FirstOrDefault(c => c.Type == "Guid")?.Value;
        try
        {
            if (leaveRequest.FileUpload != null && leaveRequest.FileUpload.Length > 0)
            {
                // Generate a unique file name
                string fileName = Guid.Parse(guid) + Path.GetExtension(leaveRequest.FileUpload.FileName);

                // Construct the full path within the wwwroot folder
                string uploadPath = Path.Combine("uploads", $"{guid}", fileName);
                string fullPath = Path.Combine(_env.WebRootPath, uploadPath);

                // Ensure the directory exists before attempting to save the file
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await leaveRequest.FileUpload.CopyToAsync(stream);
                }

                // Image successfully uploaded
                ViewBag.Message = "Upload successful";

                leaveRequest.Attachment = "/" + uploadPath;
            }
            else
            {
                ViewBag.Message = "No file selected";
            }
        }
        catch (Exception ex)
        {
            ViewBag.Message = $"Error: {ex.Message}";
        }
        
        RegisterLeaveDto leaveRequestDto = leaveRequest;
        var emp = await _employeeRepository.Get();
        
        var data = emp.Data.FirstOrDefault(e => e.Guid == Guid.Parse(guid));

        var checkDays = new CheckDaysHandler();
        var leaveDays = checkDays.Get(leaveRequestDto.LeaveStart, leaveRequestDto.LeaveEnd);
        
        if (data.LeaveRemain >= leaveDays)
        {
            var register = new RegisterLeaveRequestDto()
            {
                LeaveType = leaveRequest.LeaveType,
                LeaveStart = leaveRequest.LeaveStart,
                LeaveEnd = leaveRequest.LeaveEnd,
                Notes = leaveRequest.Notes,
                Attachment = leaveRequest.Attachment
            };

            var result = await _repository.RegisterLeave(register);

            if (result.Code == 200)
            {
                TempData["Success"] = $"Create Leave ";
                return RedirectToAction("Account", "LeaveRequest");
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
    [Authorize(Roles = "employee")]
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

    [HttpPost]
    [Authorize(Roles = "employee")]
    public async Task<IActionResult> UpdateEmployee(LeaveRequestDto leave)
    {
        var result = await _repository.Put(leave.Guid, leave);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Updated! - {result.Message}!";
            return RedirectToAction("Account", "LeaveRequest");
        }
        TempData["Error"] = $"Data failed to Update!";
        return RedirectToAction("EditEmployee","LeaveRequest");
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
                var checkDays = new CheckDaysHandler();
                var leaveDays = checkDays.Get(entity.LeaveStart, entity.LeaveEnd);
                data.LeaveRemain = data.LeaveRemain - leaveDays;

                var udate = await _employeeRepository.Put(data.Guid, data);
            }
        }
        catch
        {
            // ignored
        }

        return RedirectToAction("Index", "LeaveRequest");
    }

    /* [HttpPost]
     public async Task<IActionResult> GetOnProcessRequests()
     {
         var onProcessRequests = _dbContext.Requests.Where(r => r.Status == "OnProcess").ToList();
         return Ok(onProcessRequests);
     }*/

}


    
