using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs.Feedbacks;
using Server.Services;
using Server.Utilities.Handler;

namespace Server.Controllers;

[ApiController]
[Route("api/feedbacks")]
/*[Authorize(Roles = "manager")]*/
public class FeedbackController : ControllerBase
{
    private readonly FeedbackService _feedbackService;
    
    public FeedbackController(FeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _feedbackService.GetAll();
        var feedbackDtos = result.ToList();
        if (!feedbackDtos.Any())
        {
            return NotFound(new ResponseHandler<FeedbackDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<FeedbackDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieving data",
            Data = feedbackDtos
        });
    }
    
    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _feedbackService.GetByGuid(guid);
        if (result is null)
        {
            return NotFound(new ResponseHandler<FeedbackDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        return Ok(new ResponseHandler<FeedbackDto>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Success retrieving data",
                Data = result
            }
        );
    }

    [HttpPost]
    public IActionResult Insert(NewFeedbackDto newFeedbackDto)
    {
        var result = _feedbackService.Create(newFeedbackDto);
        if (result is null)
        {
            return StatusCode(500, new ResponseHandler<NewFeedbackDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });
        }

        return Ok(new ResponseHandler<NewFeedbackDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieving data",
            Data = result
        });
    }
    
    [HttpPut]
    public IActionResult Update(FeedbackDto feedbackDto)
    {
        var result = _feedbackService.Update(feedbackDto);
        if (result is -1)
        {
            return NotFound(new ResponseHandler<FeedbackDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<FeedbackDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });
        }

        return Ok(new ResponseHandler<FeedbackDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieving data",
            Data = feedbackDto
        });
    }
    
    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var result = _feedbackService.Delete(guid);
        if (result is -1)
        {
            return NotFound(new ResponseHandler<FeedbackDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Guid is not found"
            });
        }

        if (result is 0)
        {
            return StatusCode(500, new ResponseHandler<FeedbackDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });
        }

        return Ok(new ResponseHandler<FeedbackDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Success retrieving data"
        });
    }
}