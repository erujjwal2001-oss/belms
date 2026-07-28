using BELMS.Application.Interfaces.IRepo;
using BELMS.Domain.Entities;
using BELMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BELMS.Infrastructure.Repositories;

public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    private readonly AppDbContext context = context;

    //  dynamic filtering, pagination, sorting
    public IQueryable<Employee> Query()
    {
        return context.Employees
            .Where(x => !x.IsDeleted);
    }


    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await context.Employees
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }


    public async Task<Employee?> GetByEmailAsync(string email)
    {
        return await context.Employees
            .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
    }

    public async Task<Employee?> GetByEmployeeCodeAsync(string employeeCode)
    {
        return await context.Employees
            .FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode && !x.IsDeleted);
    }

    public async Task<int> CountAsync()
    {
        return await context.Employees.CountAsync(x => !x.IsDeleted);
    }

    public async Task AddAsync(Employee employee)
    {
        await context.Employees.AddAsync(employee);
    }


    public Task UpdateAsync(Employee employee)
    {
        context.Employees.Update(employee);
        return Task.CompletedTask;
    }

    // ✅ Delete (soft delete recommended)
    public Task DeleteAsync(Employee employee)
    {
        employee.IsDeleted = true;
        context.Employees.Update(employee);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}