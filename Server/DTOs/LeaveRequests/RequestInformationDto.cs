namespace Server.DTOs.LeaveRequests;

public class RequestInformationDto
{
    public string Requester { get; set; }
    public string Department { get; set; }
    public int AvailableLeave { get; set; }
    public int TotalLeave { get; set; }
    public int EligibleLeave { get; set; }
}