using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.LeaveRequests;
using Client.Models;
using Server.Contracts;
using Server.Utilities.Enums;
using Server.Utilities.Handler;
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
        var employees = await _employeeRepository.Get();
        var employee = employees.Data.FirstOrDefault(e => e.Guid == leaveRequest.Data.EmployeeGuid);
        if (result.Code == 200)
        {
            var userEmail = User.FindFirst("Email")?.Value;
            if (!string.IsNullOrEmpty(userEmail))
            {
                var subject = "Leave Request - Created";
                // Membuat konten HTML
                var content = $@"<h1>Permohonan Cuti - Dibuat</h1>
                            <table>
                                <tr>
                                    <th>Informasi</th>
                                    <th>Detail</th>
                                </tr>
                                <tr>
                                    <td><strong>Nama </strong></td>
                                    <td>{employee.FirstName} {employee.LastName}</td>
                                </tr>
                                <tr>
                                    <td><strong>Nik </strong></td>
                                    <td>{employee.Nik}</td>
                                </tr>
                                <tr>
                                    <td><strong>Nomor Hp </strong></td>
                                    <td>{employee.PhoneNumber}</td>
                                </tr>
                                <tr>
                                    <td><strong>Tipe Cuti</strong></td>
                                    <td>{leaveRequest.Data.LeaveType}</td>
                                </tr>
                                <tr>
                                    <td><strong>Tanggal Mulai Cuti</strong></td>
                                    <td>{leaveRequest.Data.LeaveStart:dddd, dd/MM/yyyy}</td>
                                </tr>
                                <tr>
                                    <td><strong>Tanggal Berakhir Cuti</strong></td>
                                    <td>{leaveRequest.Data.LeaveEnd:dddd, dd/MM/yyyy}</td>
                                </tr>
                                <tr>
                                    <td><strong>Catatan</strong></td>
                                    <td>{leaveRequest.Data.Notes}</td>
                                </tr>
                                <tr>
                                    <td><strong>Link Lampiran</strong></td>
                                    <td><a href='{_env.WebRootPath}{leaveRequest.Data.AttachmentUrl}'>Attachment</a></td>
                                </tr>
                            </table>
                            <p>Terima kasih atas permohonan cuti Anda. Kami akan memprosesnya segera.</p>
                            <p>Harap jangan ragu untuk menghubungi kami jika Anda memiliki pertanyaan lebih lanjut.</p>
                            <br>
                            <p>Salam,</p>
                            <p>Leave Request Management System</p>
";

                var body = Body(content);
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
        var listRequest = new EditLeaveRequestDto();
        var data = result.Data;
        if (result.Data != null)
        {
            listRequest = (EditLeaveRequestDto)result.Data;
        }
        return View(listRequest);
    }

    [HttpPost]
    [Authorize(Roles = "employee")]
    public async Task<IActionResult> EditEmployee(EditLeaveRequestDto leave)
    {
        var userClaims = User.Claims;
        var guid = userClaims.FirstOrDefault(c => c.Type == "Guid")?.Value;
        try
        {
            if (leave.FileUpload != null && leave.FileUpload.Length > 0)
            {
                // Generate a unique file name
                string fileName = Guid.Parse(guid) + Path.GetExtension(leave.FileUpload.FileName);

                // Construct the full path within the wwwroot folder
                string uploadPath = Path.Combine("uploads", $"{guid}", fileName);
                string fullPath = Path.Combine(_env.WebRootPath, uploadPath);

                // Ensure the directory exists before attempting to save the file
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await leave.FileUpload.CopyToAsync(stream);
                }

                // Image successfully uploaded
                TempData["Success"] = "Upload successful";

                leave.Attachment = "/" + uploadPath;
            }
            else
            {
                
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error: {ex.Message}";
        }
        
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
                data.LeaveRemain -= leaveDays;

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
                    var attachmentLink = $"{_env.WebRootPath}{entity.AttachmentUrl}";

                    var subject = "Pengajuan Cuti Anda";

                    // Membuat konten HTML
                    var content = $@"<h1>Pengajuan Cuti Anda</h1>
                                <table>
                                    <tr>
                                        <th>Status Pengajuan Cuti</th>
                                        <td><strong>{status}</strong></td>
                                    </tr>
                                    <tr>
                                        <th colspan='2'>Berikut ini adalah rincian pengajuan cuti Anda:</th>
                                    </tr>
                                    <tr>
                                        <th>Tipe Cuti</th>
                                        <td>{leaveType}</td>
                                    </tr>
                                    <tr>
                                        <th>Tanggal Mulai Cuti</th>
                                        <td>{leaveStart:dddd, dd/MM/yyyy}</td>
                                    </tr>
                                    <tr>
                                        <th>Tanggal Berakhir Cuti</th>
                                        <td>{leaveEnd:dddd, dd/MM/yyyy}</td>
                                    </tr>
                                    <tr>
                                        <th>Notes</th>
                                        <td>{notes}</td>
                                    </tr>
                                    <!-- Jika Anda memiliki data attachment, Anda bisa menambahkannya di sini -->
                                    <tr>
                                        <th>Attachment</th>
                                        <td><a href='{attachmentLink}'>Download Attachment</a></td>
                                    </tr>
                                </table>
                                <p>Terima kasih atas pengajuan cuti Anda.</p>
                                <p>Harap jangan ragu untuk menghubungi kami jika Anda memiliki pertanyaan lebih lanjut.</p>
                                <br>
                                <p>Salam,</p>
                                <p>Leave Request Management System</p>";

                    var managerContent = $@"
                        <h1>Pengajuan Cuti Karyawan</h1>
                        <table>
                            <tr>
                                <th>Nama Karyawan</th>
                                <td><strong>{data.FirstName} {data.LastName}</strong></td>
                            </tr>
                            <tr>
                                <th>Status Pengajuan Cuti</th>
                                <td><strong>{status}</strong></td>
                            </tr>
                            <tr>
                                <th colspan='2'>Berikut ini adalah rincian pengajuan cuti karyawan:</th>
                            </tr>
                            <tr>
                                <th>Tipe Cuti</th>
                                <td>{leaveType}</td>
                            </tr>
                            <tr>
                                <th>Tanggal Mulai Cuti</th>
                                <td>{leaveStart:dddd, dd/MM/yyyy}</td>
                            </tr>
                            <tr>
                                <th>Tanggal Berakhir Cuti</th>
                                <td>{leaveEnd:dddd, dd/MM/yyyy}</td>
                            </tr>
                            <tr>
                                <th>Notes</th>
                                <td>{notes}</td>
                            </tr>
                            <!-- Jika karyawan memiliki data attachment, Anda bisa menambahkannya di sini -->
                            <tr>
                                <th>Attachment</th>
                                <td><a href='{attachmentLink}'>Download Attachment</a></td>
                            </tr>
                        </table>
                        <p>Silakan tinjau pengajuan cuti karyawan ini dan ambil tindakan yang sesuai.</p>
                        <p>Jangan ragu untuk menghubungi karyawan ini jika Anda memiliki pertanyaan lebih lanjut.</p>
                        <br>
                        <p>Salam,</p>
                        <p>Leave Request Management System</p>";

                    var bodyManager = Body(managerContent);
                    var body = Body(content);

                    // Mengirim email dengan HTML
                    _emailHandler.SendEmail(userEmail, subject, bodyManager);
                    _emailHandler.SendEmail(data.Email, subject, body);
                }
            }
        }
        catch
        {
            // ignored
        }

        return RedirectToAction("Index", "LeaveRequest");
    }

    public string Body(string content)
{
    return $@"
<!DOCTYPE html>
<html>
<head>
    <title>Leave Request - Created</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background: #f0f0f0;
            background-size: cover;
        }}
        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: rgba(255, 255, 255, 0.9);
            border-radius: 10px;
            backdrop-filter: blur(15px);
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }}
        header {{
            color: white;
            text-align: center;
            padding: 10px 0;
            border-bottom: 2px solid #603838;
        }}
        footer {{
            color: white;
            text-align: center;
            padding: 10px 0;
            border-top: 2px solid #574040;
        }}
        h1 {{
            color: #007BFF;
            text-align: center;
        }}
        table {{
            width: 100%;
        }}
        table, th, td {{
            border: 1px solid #ddd;
            border-collapse: collapse;
        }}
        th, td {{
            padding: 8px;
            text-align: left;
        }}
        th {{
            background-color: #f2f2f2;
        }}
        a {{
            color: #007BFF;
            text-decoration: none;
        }}
        .footer {{
            text-align: center;
            color: #888;
        }}
        /* Style untuk gambar di header */
        .header-image {{
            display: block;
            margin: 0 auto;
            width: 100px;
            height: auto;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <header>
            <img src='file:///D:/workspace/Metrodata/GIT/leave_request/Server/uploads/7e33ae6c-69c2-4a1a-99fc-83159b22d81c_metrodata.webp' alt='Metrodata Logo' class='header-image'>
        </header>

        {content}

        <footer>
            <p class='footer'>PT. Mitra Integrasi Informatika</p>
            <p class='footer'>APL Tower Lantai 37, Jl. Letjen S. Parman Kav. 28, RT.12/RW.6,</p>
            <p class='footer'>Tj. Duren Sel., Jakarta Barat, Kota Jakarta Barat, Daerah Khusus Ibukota Jakarta 11470</p>
        </footer>
    </div>
</body>
</html>
";
}

}


    
