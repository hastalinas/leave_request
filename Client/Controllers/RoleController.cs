using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Roles;
using Server.Models;
using System.Data;
using System.Diagnostics;
using Server.Utilities.Handler;

namespace Client.Controllers;

[Authorize(Roles = "admin")]

public class RoleController : Controller
{
    private readonly IRoleRepository _repository;

    public RoleController(IRoleRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _repository.Get();
        var listRole = new List<RoleDto>();

        if (result.Data != null)
        {
            listRole = result.Data.ToList();
        }
        return View(listRole);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleDto role)
    {
        var result = await _repository.Post(role);

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
        var listRole = new RoleDto();

        if (result.Data != null)
        {
            listRole = result.Data;
        }
        return View(listRole);
    }

    [HttpPost]
    public async Task<IActionResult> Update(RoleDto role)
    {
        var result = await _repository.Put(role.Guid, role);

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
        var result = await _repository.Delete(guid);

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
