using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Client.Controllers;

public class RoleController : Controller
{
    private readonly IRoleRepository repository;

    public RoleController(IRoleRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListRole = new List<Role>();

        if (result.Data != null)
        {
            ListRole = result.Data.ToList();
        }
        return View();
    }
}
