using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Employees;
using Server.Models;
using System.Data;

namespace Client.Controllers;
[Authorize(Roles = "employee,manager, admin")]
public class EmployeeController : Controller
{
    private readonly IEmployeeRepository repository;

    public EmployeeController(IEmployeeRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListEmployee = new List<Employee>();

        if (result.Data != null)
        {
            ListEmployee = result.Data.ToList();
        }
        return View(ListEmployee);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        var result = await repository.Post(employee);

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
        var ListEmployee = new EmployeeDto();

        if (result.Data != null)
        {
            ListEmployee = (EmployeeDto)result.Data;
        }
        return View(ListEmployee);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Employee employee)
    {
        var result = await repository.Put(employee.Guid, employee);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Updated! - {result.Message}!";
            return RedirectToAction("Index", "Employee");
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

        return RedirectToAction("Index", "Employee");
    }
}
