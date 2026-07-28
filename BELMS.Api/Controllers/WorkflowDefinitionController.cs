using BELMS.Application.DTOs.Workflows;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BELMS.Api.Controllers;

[ApiController]
[Route("api/workflow-definitions")]
[Authorize(Roles = nameof(Role.Admin))]
public class WorkflowDefinitionController(IWorkflowDefinitionService workflowDefinitionService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkflowDefinitionRequest request)
    {
        var result = await workflowDefinitionService.CreateAsync(request);
        return ProcessResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await workflowDefinitionService.GetByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await workflowDefinitionService.GetAllAsync();
        return ProcessResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWorkflowDefinitionRequest request)
    {
        var result = await workflowDefinitionService.UpdateAsync(id, request);
        return ProcessResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await workflowDefinitionService.DeleteAsync(id);
        return ProcessResult(result);
    }
}
