﻿using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Employees;
using Server.DTOs.LeaveRequests;
using Server.Services;
using Server.Utilities.Handler;

namespace Server.Controllers;

[ApiController]
[Route("api/leave-request")]
// [Authorize]
public class LeaveRequestController : ControllerBase
{
    private readonly LeaveRequestService _leaveRequestService;
    private readonly EmployeeService _employeeService;
    
    public LeaveRequestController(LeaveRequestService leaveRequestService, EmployeeService employeeService)
    {
        _leaveRequestService = leaveRequestService;
        _employeeService = employeeService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _leaveRequestService.GetAll();
        var roleDtos = result.ToList();
        if (!roleDtos.Any())
        {
            return NotFound(new ResponseHandler<LeaveRequestDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<LeaveRequestDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieving data",
            Data = roleDtos
        });
    }
    
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _leaveRequestService.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseHandler<LeaveRequestDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        return Ok(new ResponseHandler<LeaveRequestDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Success retrieving data",
                Data = result
            }
        );
    }

    [HttpPost]
    public IActionResult Insert(NewLeaveRequestDto newLeaveRequestDto)
    {
        var result = _leaveRequestService.Create(newLeaveRequestDto);
        if (result is null)
        {
            return StatusCode(500, new ResponseHandler<NewLeaveRequestDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });
        }

        return Ok(new ResponseHandler<NewLeaveRequestDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieving data",
            Data = result
        });
    }
    
    [HttpPut]
    public IActionResult Update(LeaveRequestDto leaveRequestDto)
    {
        var result = _leaveRequestService.Update(leaveRequestDto);
        if (result is -1)
        {
            return NotFound(new ResponseHandler<LeaveRequestDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<LeaveRequestDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });
        }

        return Ok(new ResponseHandler<LeaveRequestDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieving data",
            Data = leaveRequestDto
        });
    }
    
    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var result = _leaveRequestService.Delete(guid);
        if (result is -1)
        {
            return NotFound(new ResponseHandler<LeaveRequestDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<LeaveRequestDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });
        }

        return Ok(new ResponseHandler<LeaveRequestDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieving data"
        });
    }

    [HttpGet("detail")]
    [Authorize]
    public IActionResult RequestInformation()
    {
        // Mendapatkan klaim-klaim dari pengguna yang terautentikasi
        var userClaims = User.Claims;

        var enumerable = userClaims.ToList();
        var guid = enumerable.FirstOrDefault(c => c.Type == "Guid")?.Value;
        var result = _leaveRequestService.RequestInformation(Guid.Parse(guid));
        if (result is null)
        {
            return NotFound(new ResponseHandler<RequestInformationDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        return Ok(new ResponseHandler<RequestInformationDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Success retrieving data",
                Data = result
            }
        );
    }
    
    [HttpPost("request")]
    [Authorize]
    public IActionResult RegisterLeave(RegisterLeaveDto registerLeaveDto)
    {
        // Mendapatkan klaim-klaim dari pengguna yang terautentikasi
        var userClaims = User.Claims;

        var enumerable = userClaims.ToList();
        var guidClaim = enumerable.FirstOrDefault(c => c.Type == "Guid")?.Value;
        // Dapatkan peran (role) pengguna dari klaim tipe "Role"
        var roles = enumerable.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        var employee = _employeeService.GetByGuid(Guid.Parse(guidClaim));
        var newRequestNumber = "";

        try
        {
            var requests = _leaveRequestService.GetAll();
            var latestRequest = requests.LastOrDefault().RequestNumber;
            var parts = latestRequest.Substring(latestRequest.Length - 3);
            newRequestNumber = $"{registerLeaveDto.LeaveType}-{employee.Nik}{DateTime.Now.Year}{(int.Parse(parts) + 1).ToString("D3")}";
        }
        catch
        {
            newRequestNumber = $"{registerLeaveDto.LeaveType}-{employee.Nik}{DateTime.Now.Year}001";
        }
        
        var register = new NewLeaveRequestDto()
        {
            EmployeeGuid = Guid.Parse(guidClaim),
            RequestNumber = newRequestNumber,
            LeaveType = registerLeaveDto.LeaveType,
            LeaveStart = registerLeaveDto.LeaveStart,
            LeaveEnd = registerLeaveDto.LeaveEnd,
            Notes = registerLeaveDto.Notes,
            Attachment = registerLeaveDto.Attachment
        };

        var result = _leaveRequestService.Create(register);
        
        if (result is null)
        {
            return NotFound(new ResponseHandler<NewLeaveRequestDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Leave request failed"
            });
        }

        return Ok(new ResponseHandler<NewLeaveRequestDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Success retrieving data",
                Data = result
            }
        );
    }
    
    [HttpGet("request")]
    [Authorize]
    public IActionResult LeaveRequestDetail()
    {
        // Mendapatkan klaim-klaim dari pengguna yang terautentikasi
        var userClaims = User.Claims;

        var enumerable = userClaims.ToList();
        var guid = enumerable.FirstOrDefault(c => c.Type == "Guid")?.Value;
        var result = _leaveRequestService.LeaveRequestDetail(Guid.Parse(guid));
        if (result is null)
        {
            return NotFound(new ResponseHandler<LeaveRequestDetailDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<LeaveRequestDetailDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Success retrieving data",
                Data = result
            }
        );
    }
    
    [HttpPut("request")]
    [Authorize]
    public IActionResult UpdateLeaveRequest(RegisterLeaveDto registerLeaveDto)
    {
        // Mendapatkan klaim-klaim dari pengguna yang terautentikasi
        var userClaims = User.Claims;

        var enumerable = userClaims.ToList();
        var guidClaim = enumerable.FirstOrDefault(c => c.Type == "Guid")?.Value;

        try
        {
            var leaveGuid = _leaveRequestService.GetByEmployeeGuid(Guid.Parse(guidClaim));

            var register = new LeaveRequestDto()
            {
                Guid = leaveGuid.ToList()[0].Guid,
                EmployeeGuid = Guid.Parse(guidClaim),
                LeaveType = registerLeaveDto.LeaveType,
                LeaveStart = registerLeaveDto.LeaveStart,
                LeaveEnd = registerLeaveDto.LeaveEnd,
                Notes = registerLeaveDto.Notes,
                AttachmentUrl = registerLeaveDto.Attachment,
                Status = registerLeaveDto.Status
            };

            var result = _leaveRequestService.Update(register);
        
            if (result is -1)
            {
                return NotFound(new ResponseHandler<NewLeaveRequestDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Leave request failed"
                });
            }

            return Ok(new ResponseHandler<NewLeaveRequestDto>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Success retrieving data"
                }
            );
            
        }
        catch 
        {
            return StatusCode(500, new ResponseHandler<LeaveRequestDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Leave Request cannot be changed because status is not 0"
            });
        }
    }
    
    [HttpGet("request-manager")]
    [Authorize(Roles = "manager")]
    public IActionResult LeaveRequestDetailManager()
    {
        // Mendapatkan klaim-klaim dari pengguna yang terautentikasi
        var userClaims = User.Claims;

        var enumerable = userClaims.ToList();
        var guid = enumerable.FirstOrDefault(c => c.Type == "Guid")?.Value;
        var result = _leaveRequestService.LeaveRequestDetailManager(Guid.Parse(guid));
        if (result is null)
        {
            return NotFound(new ResponseHandler<LeaveRequestDetailDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<LeaveRequestDetailDto>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Success retrieving data",
                Data = result
            }
        );
    }

    [HttpGet("request-with-name")]
    public IActionResult GetRequestWithName()
    {
        var result = _leaveRequestService.GetLeaveRequestWithNames();
        if (!result.Any())
        {
            return NotFound(new ResponseHandler<LeaveRequestAdminDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<LeaveRequestAdminDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieving data",
            Data = result
        });
    }


}