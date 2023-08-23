using Client.Contracts;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Feedbacks;
using Server.DTOs.Roles;
using Server.Models;
using System.Data;
using System.Diagnostics;
using Server.Utilities.Handler;

namespace Client.Controllers;

[Authorize(Roles = "manager, admin")]


public class FeedbackController : Controller
{
    private readonly IFeedbackRepository _repository;

    public FeedbackController(IFeedbackRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _repository.Get();
        var listFeedback = new List<FeedbackDto>();

        if (result.Data != null)
        {
            listFeedback = result.Data.ToList();
        }
        return View(listFeedback);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(FeedbackDto feedback)
    {
        var result = await _repository.Post(feedback);

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
        var ListFeedback = new FeedbackDto();

        if (result.Data != null)
        {
            ListFeedback = (FeedbackDto)result.Data;
        }
        return View(ListFeedback);
    }

    [HttpPost]
    public async Task<IActionResult> Update(FeedbackDto feedback)
    {
        var result = await _repository.Put(feedback.Guid, feedback);

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
