using Client.Contracts;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Accounts;

namespace Client.Controllers;

public class EmployeeLeaveReportController : Controller
{
    public readonly IAccountRepository _accountRepository;

    public EmployeeLeaveReportController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<IActionResult> Account()
    {
        var result = await _accountRepository.Get();
        List<AccountDto> listAccount = null;
        if (result.Data != null)
        {
            listAccount = result.Data.ToList();
        }
        return View(listAccount);
    }
}