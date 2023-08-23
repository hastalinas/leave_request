using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client.Controllers;
[Authorize(Roles = "employee")]

public class DashboardController : Controller
{
    public DashboardController()
    {
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpGet("/Unauthorized")]
    public IActionResult Unauthorized()
    {
        return View("Unauthorized");
    }

    [AllowAnonymous]
    [Route("/Forbidden")]
    public IActionResult Forbidden()
    {
        return View("Forbidden");
    }

    [AllowAnonymous]
    [Route("/NotFound")]
    public IActionResult Notfound()
    {
        return View("Notfound");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}