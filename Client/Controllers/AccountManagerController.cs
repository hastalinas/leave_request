using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Client.Contracts;
using Server.DTOs.Employees;

namespace Client.Controllers
{
    public class AccountManagerController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAccountRepository _accountRepository;

        public AccountManagerController(IWebHostEnvironment webHostEnvironment, IEmployeeRepository employeeRepository, IAccountRepository accountRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _employeeRepository = employeeRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userClaims = User.Claims;
            var guid = userClaims.FirstOrDefault(c => c.Type == "Guid")?.Value;
            var results =await _employeeRepository.GetAllEmployeewithName();
            var result = results.Data.FirstOrDefault(e => e.Guid == Guid.Parse(guid));
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(EmployeeWithNameDto model)
        {
            try
            {
                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    var userClaims = User.Claims;
                    var guid = userClaims.FirstOrDefault(c => c.Type == "Guid")?.Value;
    
                    // Generate a unique file name
                    string fileName = Guid.Parse(guid) + Path.GetExtension(model.ProfileImage.FileName);

                    // Construct the full path within the wwwroot folder
                    string uploadPath = Path.Combine("uploads", "profil-image", fileName);
                    string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath);

                    // Ensure the directory exists before attempting to save the file
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.ProfileImage.CopyToAsync(stream);
                    }

                    // Image successfully uploaded
                    ViewBag.Message = "Upload successful";

                    var accounts = await _accountRepository.Get();
                    var account = accounts.Data.FirstOrDefault(a => a.Guid == Guid.Parse(guid));
                    account.ProfilPictureUrl = "/" + uploadPath; // Store relative path in the database
                    await _accountRepository.Put(account.Guid, account);
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

            return View("Index");
        }

    }
}