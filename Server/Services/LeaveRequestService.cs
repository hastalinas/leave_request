using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
using Server.Contracts;
using Server.DTOs.Employees;
using Server.DTOs.LeaveRequests;
using Server.Models;
using Server.Utilities.Handler;

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
        var employeeDto = _employeeRepository.GetByGuid(guid);
        if (employeeDto is null)
        {
            return null;
        }
        
        var lastLeaveUpdate = employeeDto.LastLeaveUpdate;

        DateTime today = DateTime.Now;

        var totalDays = (today - lastLeaveUpdate).TotalDays;
        var yearsOfWork = (int)totalDays / 365;
        if (today < lastLeaveUpdate.AddYears(yearsOfWork))
        {
            yearsOfWork--; // Reduce by 1 if the hiring anniversary hasn't occurred yet this year
        }

        if (yearsOfWork >= 1)
        {
            employeeDto.LeaveRemain = employeeDto.LeaveRemain + (yearsOfWork * 12); // Add 12 days per year
            employeeDto.LastLeaveUpdate = lastLeaveUpdate.AddYears(yearsOfWork);
        }

        var result = _employeeRepository.Update(employeeDto);

        var requestInformation = (
            from employee in _employeeRepository.GetAll()
            where employee.Guid == guid
            join department in _departmentRepository.GetAll() on employee.DepartmentGuid equals department.Guid
            select new RequestInformationDto
            {
                Requester = $"{employee.Nik} - {employee.FirstName} {employee.LastName}",
                Department = $"{department.Code} - {department.Name}",
                AvailableLeave = employee.LeaveRemain, // Set your calculation here
                TotalLeave = 0,      // Set your calculation here
                EligibleLeave = 2    // Set your calculation here
            }
        ).FirstOrDefault();

        return requestInformation;
    }
    
    public IEnumerable<LeaveRequestDetailDto>? LeaveRequestDetail(Guid guid)
    {
        int requestNumber = 1; // You need to calculate this based on existing records

        var leaveRequestDetail = (
            from employee in _employeeRepository.GetAll()
            join leaveRequest in _leaveRequestRepository.GetAll()
                on employee.Guid equals leaveRequest.EmployeeGuid
            join manager in _employeeRepository.GetAll()
                on employee.ManagerGuid equals manager.Guid
            where employee.Guid == guid
            select new LeaveRequestDetailDto
            {
                Guid = leaveRequest.Guid,
                RequestNumber = $"{leaveRequest.LeaveType} - {employee.Nik}{DateTime.Now.Year}{requestNumber++}",
                RelationManager = $"{manager.Nik} - {manager.FirstName} {manager.LastName}",
                LeaveType = leaveRequest.LeaveType,
                LeaveStart = leaveRequest.LeaveStart,
                LeaveEnd = leaveRequest.LeaveEnd,
                PhoneNumber = employee.PhoneNumber,
                LeaveDays = leaveRequest.LeaveEnd - leaveRequest.LeaveStart,
                Notes = leaveRequest.Notes,
                Attachment = leaveRequest.AttachmentUrl,
                Status = leaveRequest.Status,
                FeedbackNotes = leaveRequest.FeedbackNotes
            }
        ).ToList();
    
        return leaveRequestDetail;
    }
    
    public IEnumerable<LeaveRequestDetailDto>? LeaveRequestDetailManager(Guid guid)
    {
        int requestNumber = 1; // You need to calculate this based on existing records

        var leaveRequestDetail = (
            from employee in _employeeRepository.GetAll()
            join leaveRequest in _leaveRequestRepository.GetAll()
                on employee.Guid equals leaveRequest.EmployeeGuid
            join manager in _employeeRepository.GetAll()
                on employee.ManagerGuid equals manager.Guid
            where employee.ManagerGuid == guid && employee.Guid != guid
            // The condition "employee.Guid != guid" is added to exclude the employee with the given guid.
            select new LeaveRequestDetailDto
            {
                Guid = leaveRequest.Guid,
                RequestNumber = $"{leaveRequest.LeaveType} - {employee.Nik}{DateTime.Now.Year}{requestNumber++}",
                RelationManager = $"{manager.Nik} - {manager.FirstName} {manager.LastName}",
                FullName = $"{employee.FirstName} {employee.LastName}",
                LeaveType = leaveRequest.LeaveType,
                LeaveStart = leaveRequest.LeaveStart,
                LeaveEnd = leaveRequest.LeaveEnd,
                PhoneNumber = employee.PhoneNumber,
                LeaveDays = leaveRequest.LeaveEnd - leaveRequest.LeaveStart,
                Notes = leaveRequest.Notes,
                Attachment = leaveRequest.AttachmentUrl,
                Status = leaveRequest.Status,
                FeedbackNotes = leaveRequest.FeedbackNotes
            }
        ).ToList();

    
        return leaveRequestDetail;
    }

    public IEnumerable<LeaveRequestDto> GetByEmployeeGuid(Guid guid)
    {
        var leaveRequests = from lr in _leaveRequestRepository.GetAll()
            join e in _employeeRepository.GetAll() on lr.EmployeeGuid equals e.Guid
            where e.Guid == guid && lr.Status == 0
            select new LeaveRequestDto
            {
                Guid = lr.Guid,
                EmployeeGuid = lr.EmployeeGuid,
                LeaveType = lr.LeaveType,
                LeaveStart = lr.LeaveStart,
                LeaveEnd = lr.LeaveEnd,
                Notes = lr.Notes,
                AttachmentUrl = lr.AttachmentUrl,
                Status = lr.Status
            };
        return leaveRequests;
    }

    public int CalculateAvailableLeave(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return -1;
        }
        
        var lastLeaveUpdate = employee.LastLeaveUpdate;

        DateTime today = DateTime.Now;

        var totalDays = (today - lastLeaveUpdate).TotalDays;
        var yearsOfWork = (int)totalDays / 365;
        if (today < lastLeaveUpdate.AddYears(yearsOfWork))
        {
            yearsOfWork--; // Reduce by 1 if the hiring anniversary hasn't occurred yet this year
        }

        if (yearsOfWork >= 1)
        {
            employee.LeaveRemain = employee.LeaveRemain + (yearsOfWork * 12); // Add 12 days per year
            employee.LastLeaveUpdate = lastLeaveUpdate.AddYears(yearsOfWork);
        }

        var result = _employeeRepository.Update(employee);
        
        return result? 1 : 0;
    }

    public IEnumerable<LeaveRequestAdminDto>? GetLeaveRequestWithNames()
    {
        var merge = (from employee in _employeeRepository.GetAll()
                     join leaveRequest in _leaveRequestRepository.GetAll() on employee.Guid equals leaveRequest.EmployeeGuid
                     
                     select new LeaveRequestAdminDto
                     {
                         Guid = employee.Guid,
                         EmployeeGuid = leaveRequest.EmployeeGuid,
                         FullName = employee.FirstName + " " + employee.LastName,
                         LeaveType = leaveRequest.LeaveType,
                         LeaveStart = leaveRequest.LeaveStart,
                         LeaveEnd = leaveRequest.LeaveEnd,
                         Notes = leaveRequest.Notes,
                         AttachmentUrl = leaveRequest.AttachmentUrl,
                         FeedbackNotes = leaveRequest.FeedbackNotes, 
                         Status = leaveRequest.Status,
                     }).OrderBy(LeaveRequest => LeaveRequest.LeaveStart);
        return merge;
    }

}