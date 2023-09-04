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
using Server.Contracts;
using Server.DTOs.Employees;
using Server.Utilities.Enums;
using Server.Utilities.Handler;
using Server.DTOs.AccountRoles;
using IEmployeeRepository = Client.Contracts.IEmployeeRepository;
using ILeaveRequestRepository = Client.Contracts.ILeaveRequestRepository;

namespace Client.Controllers;

public class LeaveRequestController : Controller
{
    private readonly ILeaveRequestRepository _repository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IWebHostEnvironment _env;
    private readonly IEmailHandler _emailHandler;
    public LeaveRequestController(ILeaveRequestRepository repository, IEmployeeRepository employeeRepository, IWebHostEnvironment env, IEmailHandler emailHandler)
    {
        _repository = repository;
        _employeeRepository = employeeRepository;
        _env = env;
        _emailHandler = emailHandler;
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
                AttachmentUrl  = employee.AttachmentUrl,
                Status = employee.Status,
                FeedbackNotes = employee.FeedbackNotes
            };
            listRequest.Add(LeaveRequestNewList);
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
        var employees = await _employeeRepository.Get();
        var employee = employees.Data.FirstOrDefault(e => e.Guid == leaveRequest.Data.EmployeeGuid);
        if (result.Code == 200)
        {
            var userEmail = User.FindFirst("Email")?.Value;
            if (!string.IsNullOrEmpty(userEmail))
            {
                var subject = "Leave Request - Created";
                // Membuat konten HTML
                var body = "<!DOCTYPE html>";
                body += "<html>";
                body += "<head>";
                body += "<title>Leave Request - Created</title>";
                body += "<style>";
                body += "body {";
                body += "  font-family: Arial, sans-serif;";
                body += "  background: #f0f0f0;";
                body += "  background-size: cover;";
                body += "}";
                body += ".container {";
                body += "  max-width: 600px;";
                body += "  margin: 0 auto;";
                body += "  padding: 20px;";
                body += "  background-color: rgba(255, 255, 255, 0.9);";
                body += "  border-radius: 10px;";
                body += "  backdrop-filter: blur(15px);";
                body += "  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);";
                body += "}";
                body += "h1 {";
                body += "  color: #007BFF;";
                body += "  text-align: center;";
                body += "}";
                body += "ul {";
                body += "  list-style-type: none;";
                body += "  padding: 0;";
                body += "}";
                body += "li {";
                body += "  margin-bottom: 10px;";
                body += "}";
                body += "strong {";
                body += "  font-weight: bold;";
                body += "}";
                body += "a {";
                body += "  color: #007BFF;";
                body += "  text-decoration: none;";
                body += "}";
                body += ".footer {";
                body += "  text-align: center;";
                body += "  color: #888;";
                body += "}";
                body += "</style>";
                body += "</head>";
                body += "<body>";
                body += "<div class='container'>";
                body += "<h1>Permohonan Cuti - Dibuat</h1>";
                body += "<p>Berikut ini adalah rincian permohonan cuti:</p>";
                body += "<ul>";
                body += $"<li><strong>Nama :</strong> {employee.FirstName} {employee.LastName}</li>";
                body += $"<li><strong>Nik :</strong> {employee.Nik}</li>";
                body += $"<li><strong>Nomor Hp :</strong> {employee.PhoneNumber}</li>";
                body += $"<li><strong>Tipe Cuti:</strong> {leaveRequest.Data.LeaveType}</li>";
                body += $"<li><strong>Tanggal Mulai Cuti:</strong> {leaveRequest.Data.LeaveStart}</li>";
                body += $"<li><strong>Tanggal Berakhir Cuti:</strong> {leaveRequest.Data.LeaveEnd}</li>";
                body += $"<li><strong>Catatan:</strong> {leaveRequest.Data.Notes}</li>";
                body += $"<li><strong>Link Lampiran:</strong> <a href='{leaveRequest.Data.AttachmentUrl}'>Attachment</a></li>";
                body += "</ul>";
                body += "<p>Terima kasih atas permohonan cuti Anda. Kami akan memprosesnya segera.</p>";
                body += "<p>Harap jangan ragu untuk menghubungi kami jika Anda memiliki pertanyaan lebih lanjut.</p>";
                body += "<div class='footer'>";
                body += "<p>Salam,</p>";
                body += $"<p>Manager</p>";
                body += "</div>";
                body += "</div>";
                body += "</body>";
                body += "</html>";

                // Mengirim email dengan HTML
                _emailHandler.SendEmail(userEmail, subject, body);
            }
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

                var update = await _employeeRepository.Put(data.Guid, data);
                
                var userEmail = User.FindFirst("Email")?.Value;
                if (!string.IsNullOrEmpty(userEmail))
                {
                    // Informasi pengajuan cuti
                    var leaveType = entity.LeaveType;
                    var leaveStart = entity.LeaveStart;
                    var leaveEnd = entity.LeaveEnd;
                    var notes = entity.FeedbackNotes;
                    var status = entity.Status;

                    var subject = "Pengajuan Cuti Anda";

                    // Membuat konten HTML
                    var body = "<!DOCTYPE html>";
                    body += "<html>";
                    body += "<head>";
                    body += "<title>Pengajuan Cuti Anda</title>";
                    body += "<style>";
                    body += "body {";
                    body += "  font-family: Arial, sans-serif;";
                    body += "  background: #f0f0f0;";
                    body += "}";
                    body += ".container {";
                    body += "  max-width: 600px;";
                    body += "  margin: 0 auto;";
                    body += "  padding: 20px;";
                    body += "  background-color: rgba(255, 255, 255, 0.9);";
                    body += "  border-radius: 10px;";
                    body += "  backdrop-filter: blur(15px);";
                    body += "  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);";
                    body += "}";
                    body += "h1 {";
                    body += "  color: #007BFF;";
                    body += "  text-align: center;";
                    body += "}";
                    body += "ul {";
                    body += "  list-style-type: none;";
                    body += "  padding: 0;";
                    body += "}";
                    body += "li {";
                    body += "  margin-bottom: 10px;";
                    body += "}";
                    body += "strong {";
                    body += "  font-weight: bold;";
                    body += "}";
                    body += "a {";
                    body += "  color: #007BFF;";
                    body += "  text-decoration: none;";
                    body += "}";
                    body += ".footer {";
                    body += "  text-align: center;";
                    body += "  color: #888;";
                    body += "}";
                    body += "</style>";
                    body += "</head>";
                    body += "<body>";
                    body += "<div class='container'>";
                    body += "<h1>Pengajuan Cuti Anda</h1>";
                    body += "<p>Status Pengajuan Cuti: <strong>" + status + "</strong></p>";
                    body += "<p>Berikut ini adalah rincian pengajuan cuti Anda:</p>";
                    body += "<ul>";
                    body += $"<li><strong>Tipe Cuti:</strong> {leaveType}</li>";
                    body += $"<li><strong>Tanggal Mulai Cuti:</strong> {leaveStart}</li>";
                    body += $"<li><strong>Tanggal Berakhir Cuti:</strong> {leaveEnd}</li>";
                    body += $"<li><strong>Notes:</strong> {notes}</li>";
                    body += "</ul>";
                    body += "<p>Terima kasih atas pengajuan cuti Anda.</p>";
                    body += "<p>Harap jangan ragu untuk menghubungi kami jika Anda memiliki pertanyaan lebih lanjut.</p>";
                    body += "<div class='footer'>";
                    body += "<p>Salam,</p>";
                    body += $"<p>Manager</p>";
                    body += "</div>";
                    body += "</div>";
                    body += "</body>";
                    body += "</html>";

                    // Mengirim email dengan HTML
                    _emailHandler.SendEmail(userEmail, subject, body);
                }
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


    
