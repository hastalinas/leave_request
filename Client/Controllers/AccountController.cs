using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.DTOs.Accounts;
using Server.Models;
using Server.Utilities.Handler;
using System.Diagnostics;
using System.Net;


namespace Client.Controllers;
public class AccountController : Controller
{
    private readonly IAccountRepository repository;
    //private readonly LeaveDbContext dbContext;
   public AccountController(IAccountRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListAccount = new List<Account>();

        if (result.Data != null)
        {
            ListAccount = result.Data.ToList();
        }
        return View();
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
        var result = await repository.Login(login);
        if (result is null)
        {
            //TempData["Error"] = $"Failed to Login! - {result.Message}!";
            return RedirectToAction("Login", "Accoount");
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
       
        var result = await repository.Register(register);
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
            var result = await repository.ForgotPassword(forgotPassword);
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
            var result = await repository.ChangePassword(changePassword); // You need to implement this method in your repository

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