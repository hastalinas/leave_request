using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.LeaveRequests;
using Server.Services;
using Server.Utilities.Handler;

namespace Server.Controllers;

[ApiController]
[Route("api/leave-request")]
[Authorize]
public class LeaveRequestController : ControllerBase
{
    private readonly LeaveRequestService _leaveRequestService;
    
    public LeaveRequestController(LeaveRequestService leaveRequestService)
    {
        _leaveRequestService = leaveRequestService;
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

    [HttpGet("{guid}/detail")]
    public IActionResult RequestInformation(Guid guid)
    {
        var result = _leaveRequestService.RequestInformation(guid);
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
}