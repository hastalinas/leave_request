using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Roles;
using Server.Models;
using System.Data;
using System.Diagnostics;

namespace Client.Controllers;

[Authorize(Roles = "admin")]

public class RoleController : Controller
{
    private readonly IRoleRepository repository;

    public RoleController(IRoleRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListRole = new List<Role>();

        if (result.Data != null)
        {
            ListRole = result.Data.ToList();
        }
        return View(ListRole);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Role role)
    {
        var result = await repository.Post(role);

        if (result.Code == 200)
        {
            RedirectToAction("Index");
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await repository.Get(id);
        var ListRole = new RoleDto();

        if (result.Data != null)
        {
            ListRole = (RoleDto)result.Data;
        }
        return View(ListRole);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Role role)
    {
        var result = await repository.Put(role.Guid, role);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Updated! - {result.Message}!";
            return RedirectToAction("Index", "Role");
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
            TempData["Error"] = $"Failed to Delete Data - {result.Message}!";
        }

        return RedirectToAction("Index", "Role");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
