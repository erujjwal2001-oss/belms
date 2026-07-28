using BELMS.Application.DTOs.Employees;
using BELMS.Application.Filters;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BELMS.Api.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize(Roles = $"{nameof(Role.HR)},{nameof(Role.Admin)},{nameof(Role.Manager)}")]
public class EmployeeController(IEmployeeService employeeService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        var result = await employeeService.CreateAsync(request);
        return ProcessResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await employeeService.GetByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] EmployeeFilterRequest request)
    {
        var result = await employeeService.GetAllAsync(request);
        return ProcessResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest request)
    {
        var result = await employeeService.UpdateAsync(id, request);
        return ProcessResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await employeeService.DeleteAsync(id);
        return ProcessResult(result);
    }
}
