using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.AccountRoles;
using Server.DTOs.Departments;
using Server.Models;
using System.Data;

namespace Client.Controllers;

[Authorize(Roles = "admin")]


public class AccountRoleController : Controller
{
    private readonly IAccountRoleRepository repository;
  
    public AccountRoleController(IAccountRoleRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListAccountRole = new List<AccountRole>();

        if (result.Data != null)
        {
            ListAccountRole = result.Data.ToList();
        }
        return View(ListAccountRole);
    }
    [HttpGet]
    public async Task<IActionResult> Info()
    {
        var result = await repository.accountRoleInfo();
        var ListAccountRole = new List<AccountRoleInfoDto>();

        if (result.Data != null)
        {
            ListAccountRole = result.Data.ToList();
        }
        return View(ListAccountRole);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AccountRole accountRole)
    {
        var result = await repository.Post(accountRole);

        if (result.Code == 200)
        {
            RedirectToAction("Index");
        }
        return RedirectToAction(nameof(Index));
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
            TempData["Error"] = $"Failed to Delete Data - {result.Message}!";
        }

        return RedirectToAction("Index", "AccountRole");
    }
}
