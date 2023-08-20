using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Accounts;
using Server.Models;
using Server.Utilities.Handler;
using System.Diagnostics;
using System.Net;


namespace Client.Controllers;
public class AccountController : Controller
{
    private readonly IAccountRepository repository;

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
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewAccountDto newAccount)
    {

        var result = await repository.Post(newAccount);
        if (result.Status == "200")
        {
            TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
            return RedirectToAction(nameof(Index));
        }
        else if (result.Status == "409")
        {
            TempData["Error"] = $"Data failed Registered! - {result.Message}!";
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        return RedirectToAction(nameof(Index));

    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await repository.Get(id);
        var ListAccount = new AccountDto();

        if (result.Data != null)
        {
            ListAccount = (AccountDto)result.Data;
        }
        return View((AccountDto)ListAccount);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Account account)
    {
        var result = await repository.Put(account.Guid, account);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Edited! - {result.Message}!";
            return RedirectToAction("Index", "Account");
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
            TempData["Error"] = $"Data failed Deleted! - {result.Message}!";
        }
        return RedirectToAction(nameof(Index));
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}