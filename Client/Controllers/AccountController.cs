using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.DTOs.Accounts;
using Server.DTOs.Departments;
using Server.Models;
using System.Diagnostics;

namespace Client.Controllers;
public class AccountController : Controller
{
    private readonly IAccountRepository _repository;

    public AccountController(IAccountRepository repository)
    {
        repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _repository.Get();
        var ListAccount = new List<Account>();

        if (result.Data != null)
        {
            ListAccount = result.Data.ToList();
        }
        return View(ListAccount);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _repository.Get(id);
        var ListAccount = new AccountDto();

        if (result.Data != null)
        {
            ListAccount = (AccountDto)result.Data;
        }
        return View(ListAccount);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Account account)
    {
        var result = await _repository.Put(account.Guid, account);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Updated! - {result.Message}!";
            return RedirectToAction("Index", "Account");
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

        return RedirectToAction("Index", "Account");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto login)
    {
        var result = await _repository.Login(login);
        if (result is null)
        {
            //TempData["Error"] = $"Failed to Login! - {result.Message}!";
            return RedirectToAction("Login", "Account");
        }
        else if (result.Code == 409)
        {
            //TempData["Error"] = $"Failed to Login! - {result.Message}!";
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        else if (result.Code == 200)
        {
            //TempData["Success"] = $"Successfully Login! - {result.Data.Token}!";
            HttpContext.Session.SetString("JWToken", result.Data.Token);
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        //var departmentCodes = dbContext.Departments.Select(d => d.Code).ToList();
        //ViewBag.DepartmentCodes = departmentCodes;
        return View();
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDto register)
    {
        var result = await _repository.Register(register);
        if (result is null)
        {
            return RedirectToAction("Error", "Home");
        }
        else if (result.Code == 409)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            TempData["Error"] = $"Something Went Wrong! - {result.Message}!";
            return View();
        }
        else if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
            return RedirectToAction("Login", "Account");
        }
        return View(register);
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPassword)
    {
        if (ModelState.IsValid)
        {
            var result = await _repository.ForgotPassword(forgotPassword);
            if (result is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (result.Code == 404)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                TempData["Error"] = $"User not found! - {result.Message}!";
                return View();
            }
            else if (result.Code == 200)
            {
                TempData["Success"] = $"Password reset link has been sent to your email! - {result.Message}!";
                return RedirectToAction("Login", "Account");
            }
        }
        return View(forgotPassword); 
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword)
    {
        if (ModelState.IsValid)
        {
            var result = await _repository.ChangePassword(changePassword); // You need to implement this method in your repository

            if (result is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (result.Code == 400)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                TempData["Error"] = $"Invalid request! - {result.Message}!";
                return View();
            }
            else if (result.Code == 404)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                TempData["Error"] = $"User not found! - {result.Message}!";
                return View();
            }
            else if (result.Code == 200)
            {
                TempData["Success"] = $"Password has been successfully changed! - {result.Message}!";
                return RedirectToAction("Login", "Account");
            }
        }
        return View(changePassword);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}