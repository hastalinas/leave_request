using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Departments;
using Server.DTOs.LeaveRequests;
using Server.Models;
using System.Data;

namespace Client.Controllers;

[Authorize(Roles = "employee, manager, admin")]
public class LeaveRequestController : Controller
{
    private readonly ILeaveRequestRepository repository;
    public LeaveRequestController(ILeaveRequestRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListRequest = new List<LeaveRequest>();

        if (result.Data != null)
        {
            ListRequest = result.Data.ToList();
        }
        return View(ListRequest);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(LeaveRequest leaveRequest)
    {
        var result = await repository.Post(leaveRequest);

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
        var ListRequest = new LeaveRequestDto();

        if (result.Data != null)
        {
            ListRequest = (LeaveRequestDto)result.Data;
        }
        return View(ListRequest);
    }

    [HttpPost]
    public async Task<IActionResult> Update(LeaveRequest leave)
    {
        var result = await repository.Put(leave.Guid, leave);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Updated! - {result.Message}!";
            return RedirectToAction("Index", "LeaveRequests");
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

        return RedirectToAction("Index", "LeaveRequests");
    }
}
