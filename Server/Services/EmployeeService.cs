using Server.Contracts;
using Server.DTOs.Employees;
using Server.DTOs.LeaveRequests;
using Server.Models;
using Server.Utilities.Handler;

namespace Server.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILeaveRequestRepository _leaveRequestRepository;

    public EmployeeService(IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        ILeaveRequestRepository leaveRequestRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _leaveRequestRepository = leaveRequestRepository;
    }
    
    public IEnumerable<EmployeeDto> GetAll()
    {
        var employees = _employeeRepository.GetAll();
        var enumerable = employees.ToList();
        if (!enumerable.Any())
        {
            return Enumerable.Empty<EmployeeDto>();
        }

        var employeeDtos = new List<EmployeeDto>();
        enumerable.ToList().ForEach(employee => employeeDtos.Add((EmployeeDto)employee));

        return employeeDtos;
    }

    public EmployeeDto? GetByGuid(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return null;
        }

        return (EmployeeDto)employee;
    }

    public NewEmployeeDto? Create(NewEmployeeDto newEmployeeDto)
    {
        var lastNik = _employeeRepository.GetLastNik();
        Employee emp = newEmployeeDto;
        emp.Nik = GenerateHandler.Nik(lastNik);
        
        var employee = _employeeRepository.Create(emp);
        if (employee is null)
        {
            return null;
        }

        return (NewEmployeeDto)employee;
    }

    public int Update(EmployeeDto employeeDto)
    {
        var employee = _employeeRepository.GetByGuid(employeeDto.Guid);
        if (employee is null)
        {
            return -1;
        }

        Employee toUpdate = employeeDto;
        toUpdate.CreatedDate = employee.CreatedDate;
        var result = _employeeRepository.Update(toUpdate);

        return result ? 1 : 0;
    }

    public int Delete(Guid guid)
    {
        var employee = _employeeRepository.GetByGuid(guid);
        if (employee is null)
        {
            return -1;
        }

        var result = _employeeRepository.Delete(employee);
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
                RequestNumber = $"{leaveRequest.LeaveType} - {employee.Nik}{DateTime.Now.Year}{requestNumber++}",
                RelationManager = $"{manager.Nik} - {manager.FirstName} {manager.LastName}",
                LeaveType = leaveRequest.LeaveType,
                LeaveStart = leaveRequest.LeaveStart,
                LeaveEnd = leaveRequest.LeaveEnd,
                PhoneNumber = employee.PhoneNumber,
                Notes = leaveRequest.Notes,
                Attachment = leaveRequest.Attachment
            }
        ).ToList();
    
        return leaveRequestDetail;
    }
}