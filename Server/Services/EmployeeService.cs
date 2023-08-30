using Server.Contracts;
using Server.DTOs.Employees;
using Server.DTOs.LeaveRequests;
using Server.Models;
using Server.Repositories;
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

    public IEnumerable<EmployeeWithNameDto>? GetEmployeeWithNames()
    {
        var merge = (from employee in _employeeRepository.GetAll()
                    join manager in _employeeRepository.GetAll() on employee.ManagerGuid equals manager.Guid into ManagerGroup
                    from manager in ManagerGroup.DefaultIfEmpty()
                    join department in _departmentRepository.GetAll() on employee.DepartmentGuid equals department.Guid

                    select new EmployeeWithNameDto
                    {
                        Guid = employee.Guid,
                        DepartmentGuid = department.Guid,
                        LastLeaveUpdate = DateTime.Now,
                        ManagerGuid = employee.ManagerGuid,
                        Nik = employee.Nik,
                        FullName = employee.FirstName + " " + employee.LastName,
                        BirthDate = employee.BirthDate,
                        Gender = employee.Gender,
                        HiringDate = employee.HiringDate,
                        Email = employee.Email,
                        PhoneNumber = employee.PhoneNumber,
                        DepartmentName = department.Name,
                        ManagerName = manager!=null? manager.FirstName + " " + manager.LastName:null,
                        LeaveRemain = employee.LeaveRemain
                    }).OrderBy(employee => employee.Nik);
        return merge;
    }
}

