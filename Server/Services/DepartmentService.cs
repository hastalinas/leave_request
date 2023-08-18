using Server.Contracts;
using Server.DTOs.Departments;
using Server.Models;

namespace Server.Services;

public class DepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public IEnumerable<DepartmentDto> GetAll()
    {
        var departments = _departmentRepository.GetAll();
        var enumerable = departments.ToList();
        if (!enumerable.Any())
        {
            return Enumerable.Empty<DepartmentDto>();
        }

        var departmentDtos = new List<DepartmentDto>();
        enumerable.ToList().ForEach( accountRole => departmentDtos.Add((DepartmentDto)accountRole));

        return departmentDtos;
    }

    public DepartmentDto? GetByGuid(Guid guid)
    {
        var department = _departmentRepository.GetByGuid(guid);
        if (department is null)
        {
            return null;
        }

        return (DepartmentDto)department;
    }

    public NewDepartmentDto? Create(NewDepartmentDto newDepartmentDto)
    {
        var department = _departmentRepository.Create(newDepartmentDto);
        if (department is null)
        {
            return null;
        }

        return (NewDepartmentDto)department;
    }

    public int Update(DepartmentDto departmentDto)
    {
        var department = _departmentRepository.GetByGuid(departmentDto.Guid);
        if (department is null)
        {
            return -1;
        }

        Department toUpdate = departmentDto;
        toUpdate.CreatedDate = department.CreatedDate;
        var result = _departmentRepository.Update(toUpdate);

        return result ? 1 : 0;
    }

    public int Delete(Guid guid)
    {
        var department = _departmentRepository.GetByGuid(guid);
        if (department is null)
        {
            return -1;
        }

        var result = _departmentRepository.Delete(department);
        return result ? 1 : 0;
    }
}