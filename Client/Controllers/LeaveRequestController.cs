using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Client.Controllers;

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
}
