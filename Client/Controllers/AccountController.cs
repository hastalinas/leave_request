using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using Server.Data;
using Server.DTOs.Accounts;
using Server.DTOs.Departments;
using Server.Models;
using System.Diagnostics;
using Server.Utilities.Handler;

namespace Client.Controllers;
[AllowAnonymous]

public class AccountController : Controller
{
    private readonly IAccountRepository _repository;

    public AccountController(IAccountRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _repository.Get();
        var listAccount = new List<AccountDto>();

        if (result.Data != null)
        {
            listAccount = result.Data.ToList();
        }
        return View(listAccount);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AccountDto account)
    {
        var result = await _repository.Post(account);

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
        var listAccount = new AccountDto();

        if (result.Data is not null)
        {
            listAccount = (AccountDto)result.Data;
        }
        return View(listAccount);
    }

    [HttpPost]
    public async Task<IActionResult> Update(AccountDto account)
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
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login()
    {
        HttpContext.Session.Remove("JWToken");
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto login)
    {
        var result = await _repository.Login(login);
        if (result.Code == 200)
        {
            //TempData["Success"] = $"Successfully Login! - {result.Data.Token}!";
            HttpContext.Session.SetString("JWToken", result.Data.Token);
            return RedirectToAction("Index", "Dashboard");
        }
        else
        {
            TempData["Error"] = $"Failed to Login! - {result.Message}!";
            // ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto register)
    {
        var result = await _repository.Register(register);
        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
            return View("Login");
        }
        else
        {
            // ModelState.AddModelError(string.Empty, result.Message);
            TempData["Error"] = $"Something Went Wrong! - {result.Message}!";
            return View(register);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
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
                TempData["Error"] = $"{result.Message}!";
                return View();
            }
            else if (result.Code == 200)
            {
                TempData["Success"] = $"{result.Message}!";

                return RedirectToAction("ChangePassword", "Account");
            }
        }
        return View(forgotPassword); 
    }
    
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ChangePassword()
    {
        return View();
    }
    
    [HttpPost]
    [AllowAnonymous]
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
                await Task.Delay(3000);
                return View("Login");
            }
        }
        return View(changePassword);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Detail()
    {
        var result = await _repository.GetDetailAll();
        var listAccountDetail = new List<AccountDetailDto>();
        try
        {
            if (result.Data is not null)
            {
                listAccountDetail = result.Data.ToList();
            }
        }
        catch (Exception e)
        {
            return View(listAccountDetail);
        }

        return View(listAccountDetail);
    }
    
    [HttpPost]
    [Authorize(Roles = "manager")]
    public async Task<IActionResult> Activate(Guid guid)
    {
        var leaveRequest = await _repository.Get(guid);
        if (leaveRequest.Data.IsActive is true)
        {
            leaveRequest.Data.IsActive = false;
        }
        else
        {
            leaveRequest.Data.IsActive = true;
        }
        var result = await _repository.Put(guid, leaveRequest.Data);
        if (result.Code == 200)
        {
            TempData["Success"] = $"{result.Message}!";
        }
        else
        {
            TempData["Error"] = $"{result.Message}!";
        }

        return RedirectToAction("Detail", "Account");
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}