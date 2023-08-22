using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers;
using Server.Models;

namespace Client.Controllers;

public class DepartmentController : Controller
{
    private readonly IDepartmentRepository repository;

    public DepartmentController(IDepartmentRepository repository)
    {
        this.repository = repository;
    }


    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListDepartment = new List<Department>();

        if (result.Data != null)
        {
            ListDepartment = result.Data.ToList();
        }
        return View(ListDepartment);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Department department)
    {
        var result = await repository.Post(department);

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
        var ListDepartment = new Department();

        if (result.Data != null)
        {
            ListDepartment = result.Data;
        }
        return View(ListDepartment);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Department department)
    {
        var result = await repository.Put(department.Guid, department);

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
        var result = await repository.Delete(guid);

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
