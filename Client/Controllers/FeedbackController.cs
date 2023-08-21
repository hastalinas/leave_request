using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Feedbacks;
using Server.DTOs.Roles;
using Server.Models;
using System.Diagnostics;

namespace Client.Controllers;

public class FeedbackController : Controller
{
    private readonly IFeedbackRepository repository;

    public FeedbackController(IFeedbackRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListFeedback = new List<Feedback>();

        if (result.Data != null)
        {
            ListFeedback = result.Data.ToList();
        }
        return View(ListFeedback);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Feedback feedback)
    {
        var result = await repository.Post(feedback);

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
        var ListFeedback = new FeedbackDto();

        if (result.Data != null)
        {
            ListFeedback = (FeedbackDto)result.Data;
        }
        return View(ListFeedback);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Feedback feedback)
    {
        var result = await repository.Put(feedback.Guid, feedback);

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
