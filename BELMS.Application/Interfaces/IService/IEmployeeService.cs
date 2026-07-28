using BELMS.Application.Common.Pagination;
using BELMS.Application.DTOs.Employees;
using BELMS.Application.Filters;
using BELMS.Domain.Common;

namespace BELMS.Application.Interfaces.IService;

public interface IEmployeeService
{
    Task<Result<EmployeeDto>> CreateAsync(CreateEmployeeRequest request);
    Task<Result<EmployeeDto>> GetByIdAsync(Guid id);
    Task<Result<PagedResponse<EmployeeDto>>> GetAllAsync(EmployeeFilterRequest request);
    Task<Result<EmployeeDto>> UpdateAsync(Guid id, UpdateEmployeeRequest request);
    Task<Result> DeleteAsync(Guid id);
}
