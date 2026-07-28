using BELMS.Domain.Entities;

namespace BELMS.Application.Interfaces.IRepo;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id);

    Task<Employee?> GetByEmailAsync(string email);

    Task<Employee?> GetByEmployeeCodeAsync(string employeeCode);

    Task<int> CountAsync();

    Task AddAsync(Employee employee);

    Task UpdateAsync(Employee employee);

    Task DeleteAsync(Employee employee);

    IQueryable<Employee> Query();

    Task SaveChangesAsync();
}
