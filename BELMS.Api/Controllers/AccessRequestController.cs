using BELMS.Application.DTOs.AccessRequests;
using BELMS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BELMS.Api.Controllers;

[ApiController]
[Route("api/access-requests")]
[Authorize]
public class AccessRequestController(IAccessRequestService accessRequestService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccessRequestRequest request)
    {
        var result = await accessRequestService.CreateAsync(request);
        return ProcessResult(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyRequests()
    {
        var result = await accessRequestService.GetMyRequestsAsync();
        return ProcessResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await accessRequestService.GetByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpGet]
    [Authorize(Roles = "HR,Admin,Security")]
    public async Task<IActionResult> GetAll()
    {
        var result = await accessRequestService.GetAllAsync();
        return ProcessResult(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "HR,Admin,Security")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccessRequestRequest request)
    {
        var result = await accessRequestService.UpdateAsync(id, request);
        return ProcessResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "HR,Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await accessRequestService.DeleteAsync(id);
        return ProcessResult(result);
    }
}
