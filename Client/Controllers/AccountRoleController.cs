using Client.Contracts;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Client.Controllers;

public class AccountRoleController : Controller
{
    private readonly IAccountRoleRepository repository;
  
    public AccountRoleController(IAccountRoleRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListAccountRole = new List<AccountRole>();

        if (result.Data != null)
        {
            ListAccountRole = result.Data.ToList();
        }
        return View();
    }
}
