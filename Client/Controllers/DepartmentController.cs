using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers;
using Server.Data;
using Server.Models;
using Microsoft.EntityFrameworkCore;
using Server.DTOs.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.Data;
using Server.Utilities.Handler;

namespace Client.Controllers;

[Authorize(Roles = "admin,manager")]

public class DepartmentController : Controller
{
    private readonly IDepartmentRepository _repository;

    public DepartmentController(IDepartmentRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _repository.Get();
        var listDepartment = new List<DepartmentDto>();

        if (result.Data != null)
        {
            listDepartment = result.Data.ToList();
        }
        return View(listDepartment);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(DepartmentDto department)
    {
        var result = await _repository.Post(department);

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
        var listDepartment = new DepartmentDto();

        if (result.Data != null)
        {
            listDepartment = (DepartmentDto)result.Data;
        }
        return View(listDepartment);
    }

    [HttpPost]
    public async Task<IActionResult> Update(DepartmentDto department)
    {
        var result = await _repository.Put(department.Guid, department);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Updated! - {result.Message}!";
            return RedirectToAction("Index", "Department");
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

        return RedirectToAction("Index", "Department");
    }
}
