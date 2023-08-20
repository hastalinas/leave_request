using System.ComponentModel.Design;
using Server.Contracts;
using Server.DTOs.LeaveRequests;
using Server.Models;

namespace Server.Services;

public class LeaveRequestService
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository,
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }
    
    public IEnumerable<LeaveRequestDto> GetAll()
    {
        var leaveRequests = _leaveRequestRepository.GetAll();
        var enumerable = leaveRequests.ToList();
        if (!enumerable.Any())
        {
            return Enumerable.Empty<LeaveRequestDto>();
        }

        var leaveRequestDtos = new List<LeaveRequestDto>();
        enumerable.ToList().ForEach(feedback => leaveRequestDtos.Add((LeaveRequestDto)feedback));

        return leaveRequestDtos;
    }

    public LeaveRequestDto? GetByGuid(Guid guid)
    {
        var leaveRequest = _leaveRequestRepository.GetByGuid(guid);
        if (leaveRequest is null)
        {
            return null;
        }

        return (LeaveRequestDto)leaveRequest;
    }

    public NewLeaveRequestDto? Create(NewLeaveRequestDto newLeaveRequestDto)
    {
        LeaveRequest lr = newLeaveRequestDto;

        var leaveRequest = _leaveRequestRepository.Create(lr);
        if (leaveRequest is null)
        {
            return null;
        }

        return (NewLeaveRequestDto)leaveRequest;
    }

    public int Update(LeaveRequestDto leaveRequestDto)
    {
        var role = _leaveRequestRepository.GetByGuid(leaveRequestDto.Guid);
        if (role is null)
        {
            return -1;
        }

        LeaveRequest toUpdate = leaveRequestDto;
        toUpdate.CreatedDate = role.CreatedDate;
        var result = _leaveRequestRepository.Update(toUpdate);

        return result ? 1 : 0;
    }

    public int Delete(Guid guid)
    {
        var leaveRequest = _leaveRequestRepository.GetByGuid(guid);
        if (leaveRequest is null)
        {
            return -1;
        }

        var result = _leaveRequestRepository.Delete(leaveRequest);
        return result ? 1 : 0;
    }

    public RequestInformationDto? RequestInformation(Guid guid)
    {
        var requestInformation = (
            from employee in _employeeRepository.GetAll()
            where employee.Guid == guid
            join department in _departmentRepository.GetAll() on employee.DepartmentGuid equals department.Guid
            select new RequestInformationDto
            {
                Requester = $"{employee.Nik} - {employee.FirstName} {employee.LastName}",
                Department = $"{department.Code} - {department.Name}",
                AvailableLeave = 12, // Set your calculation here
                TotalLeave = 0,      // Set your calculation here
                EligibleLeave = 2    // Set your calculation here
            }
        ).FirstOrDefault();

        return requestInformation;
    }

    public LeaveRequestDetailDto? LeaveRequestDetail(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        string nik = employee?.Nik ?? "000000";
        string year = DateTime.Now.Year.ToString();
        int requestNumber = 1; // You need to calculate this based on existing records

        // var leaveRequest = _leaveRequestRepository.GetByGuid(guid);
        
        var query = from employees in _employeeRepository.GetAll()
            join leaveRequest in _leaveRequestRepository.GetAll()
                on employee.Guid equals leaveRequest.EmployeeGuid
            select employee;

        return new LeaveRequestDetailDto()
        {
            RequestNumber = $" {nik}{year}{requestNumber:D3}",
            
        };
    }
}