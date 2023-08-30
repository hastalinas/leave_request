using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Employees;
using Server.Models;
using System.Data;
using Server.Utilities.Handler;

namespace Client.Controllers;
[Authorize(Roles = "manager, admin")]
public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _repository;

    public EmployeeController(IEmployeeRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _repository.GetAllEmployeewithName();
        var listEmployee = new List<EmployeeWithNameDto>();
        foreach (var employee in result.Data)
        {
            var EmployeeNewList = new EmployeeWithNameDto
            {
                Guid = employee.Guid,
                BirthDate = employee.BirthDate,
                DepartmentGuid = employee.DepartmentGuid,
                DepartmentName = employee.DepartmentName,
                Email = employee.Email,
                FullName = employee.FullName,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate,
                LastLeaveUpdate = employee.LastLeaveUpdate,
                LeaveRemain = employee.LeaveRemain,
                ManagerGuid = employee.ManagerGuid,
                ManagerName = employee.ManagerName,
                Nik = employee.Nik,
                PhoneNumber = employee.PhoneNumber,
            };
            listEmployee.Add(EmployeeNewList);
        }

        if (result.Data != null)
        {
            listEmployee = result.Data.ToList();
        }
        return View(listEmployee);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeDto employee)
    {
        var result = await _repository.Post(employee);

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
        var listEmployee = new EmployeeDto();

        if (result.Data != null)
        {
            listEmployee = (EmployeeDto)result.Data;
        }
        return View(listEmployee);
    }

    [HttpPost]
    public async Task<IActionResult> Update(EmployeeDto employee)
    {
        var result = await _repository.Put(employee.Guid, employee);

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
        var result = await _repository.Delete(guid);

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
