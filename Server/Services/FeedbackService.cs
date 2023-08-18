using Server.Contracts;
using Server.DTOs.Feedbacks;
using Server.Models;

namespace Server.Services;

public class FeedbackService
{
    private readonly IFeedbackRepository _feedbackRepository;

    public FeedbackService(IFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }
    
    public IEnumerable<FeedbackDto> GetAll()
    {
        var feedbacks = _feedbackRepository.GetAll();
        var enumerable = feedbacks.ToList();
        if (!enumerable.Any())
        {
            return Enumerable.Empty<FeedbackDto>();
        }

        var feedbackDtos = new List<FeedbackDto>();
        enumerable.ToList().ForEach(feedback => feedbackDtos.Add((FeedbackDto)feedback));

        return feedbackDtos;
    }

    public FeedbackDto? GetByGuid(Guid guid)
    {
        var feedback = _feedbackRepository.GetByGuid(guid);
        if (feedback is null)
        {
            return null;
        }

        return (FeedbackDto)feedback;
    }

    public NewFeedbackDto? Create(NewFeedbackDto newFeedbackDto)
    {
        Feedback fb = newFeedbackDto;

        var feedback = _feedbackRepository.Create(fb);
        if (feedback is null)
        {
            return null;
        }

        return (NewFeedbackDto)feedback;
    }

    public int Update(FeedbackDto feedbackDto)
    {
        var feedback = _feedbackRepository.GetByGuid(feedbackDto.Guid);
        if (feedback is null)
        {
            return -1;
        }

        Feedback toUpdate = feedbackDto;
        toUpdate.CreatedDate = feedback.CreatedDate;
        var result = _feedbackRepository.Update(toUpdate);

        return result ? 1 : 0;
    }

    public int Delete(Guid guid)
    {
        var feedback = _feedbackRepository.GetByGuid(guid);
        if (feedback is null)
        {
            return -1;
        }

        var result = _feedbackRepository.Delete(feedback);
        return result ? 1 : 0;
    }
}