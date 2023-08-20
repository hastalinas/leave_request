using System.Net;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.AccountRoles;
using Server.Services;
using Server.Utilities.Handler;

namespace Server.Controllers;

[ApiController]
[Route("api/account-role")]
public class AccountRoleController : ControllerBase
{
   private readonly AccountRoleService _accountRoleService;

   public AccountRoleController(AccountRoleService accountRoleService)
   {
      _accountRoleService = accountRoleService;
   }

   [HttpGet]
   public IActionResult GetAll()
   {
      var result = _accountRoleService.GetAll();
      var accountRoleDtos = result.ToList();
      if (!accountRoleDtos.Any())
      {
         return NotFound(new ResponseHandler<AccountRoleDto>()
         {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Database is empty"
         });
      }

      return Ok(new ResponseHandler<IEnumerable<AccountRoleDto>>()
      {
         Code = StatusCodes.Status200OK,
         Status = HttpStatusCode.OK.ToString(),
         Message = "Success retrieving data",
         Data = accountRoleDtos
      });
   }

   [HttpGet("{guid}")]
   public IActionResult GetByGuid(Guid guid)
   {
      var result = _accountRoleService.GetByGuid(guid);
      if (result is null)
      {
         return NotFound(new ResponseHandler<AccountRoleDto>
         {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Guid is not found"
         });
      }
      
      return Ok(new ResponseHandler<AccountRoleDto>
         {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieving data",
            Data = result
         }
      );
   }

   [HttpPost]
   public IActionResult Insert(NewAccountRoleDto newAccountRoleDto)
   {
      var result = _accountRoleService.Create(newAccountRoleDto);
      if (result is null)
      {
         return StatusCode(500, new ResponseHandler<NewAccountRoleDto>()
         {
            Code = StatusCodes.Status500InternalServerError,
            Status = HttpStatusCode.InternalServerError.ToString(),
            Message = "Error retrieving data from the database"
         });
      }
      
      return Ok(new ResponseHandler<NewAccountRoleDto>
      {
         Code = StatusCodes.Status200OK,
         Status = HttpStatusCode.OK.ToString(),
         Message = "Success retrieving data",
         Data = result
      });
   }

   [HttpPut]
   public IActionResult Update(AccountRoleDto accountRoleDto)
   {
      var result = _accountRoleService.Update(accountRoleDto);
      if (result is -1)
      {
         return NotFound(new ResponseHandler<AccountRoleDto>
         {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Guid is not found"
         });
      }
      
      if (result is 0)
      {
         return StatusCode(500, new ResponseHandler<AccountRoleDto>
         {
            Code = StatusCodes.Status500InternalServerError,
            Status = HttpStatusCode.InternalServerError.ToString(),
            Message = "Error retrieving data from the database"
         });
      }

      return Ok(new ResponseHandler<AccountRoleDto>
      {
         Code = StatusCodes.Status200OK,
         Status = HttpStatusCode.OK.ToString(),
         Message = "Success retrieving data",
         Data = accountRoleDto
      });
   }
   
   [HttpDelete]
   public IActionResult Delete(Guid guid)
   {
      var result = _accountRoleService.Delete(guid);
      if (result is -1)
      {
         return NotFound(new ResponseHandler<AccountRoleDto>
         {
            Code = StatusCodes.Status404NotFound,
            Status = HttpStatusCode.NotFound.ToString(),
            Message = "Guid is not found"
         });
      }

      if (result is 0)
      {
         return StatusCode(500, new ResponseHandler<AccountRoleDto>
         {
            Code = StatusCodes.Status500InternalServerError,
            Status = HttpStatusCode.InternalServerError.ToString(),
            Message = "Error retrieving data from the database"
         });
      }

      return Ok(new ResponseHandler<AccountRoleDto>
      {
         Code = StatusCodes.Status200OK,
         Status = HttpStatusCode.OK.ToString(),
         Message = "Success retrieving data"
      });
   }
}