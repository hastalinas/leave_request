using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.AccountRoles;
using Server.DTOs.Departments;
using Server.Models;
using System.Data;
using Server.Utilities.Handler;

namespace Client.Controllers;

[Authorize(Roles = "admin")]


public class AccountRoleController : Controller
{
    private readonly IAccountRoleRepository _repository;
  
    public AccountRoleController(IAccountRoleRepository repository)
    {
        this._repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _repository.Get();
        var listAccountRole = new List<AccountRoleDto>();

        if (result.Data != null)
        {
            listAccountRole = result.Data.ToList();
        }
        return View(listAccountRole);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AccountRoleDto accountRole)
    {
        var result = await _repository.Post(accountRole);

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
        var listAccountRole = new AccountRoleDto();

        if (result.Data != null)
        {
            listAccountRole = (AccountRoleDto)result.Data;
        }
        return View(listAccountRole);
    }

    [HttpPost]
    public async Task<IActionResult> Update(AccountRoleDto accountRole)
    {
        var result = await _repository.Put(accountRole.Guid, accountRole);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Updated! - {result.Message}!";
            return RedirectToAction("Index", "AccountRole");
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

        return RedirectToAction("Index", "AccountRole");
    }
}
