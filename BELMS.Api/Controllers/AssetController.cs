using BELMS.Application.DTOs.Assets;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BELMS.Api.Controllers;

[ApiController]
[Route("api/assets")]
[Authorize(Roles = $"{nameof(Role.IT)},{nameof(Role.HR)},{nameof(Role.Admin)}")]
public class AssetController(IAssetService assetService) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAssetRequest request)
    {
        var result = await assetService.CreateAsync(request);
        return ProcessResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await assetService.GetByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await assetService.GetAllAsync();
        return ProcessResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssetRequest request)
    {
        var result = await assetService.UpdateAsync(id, request);
        return ProcessResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await assetService.DeleteAsync(id);
        return ProcessResult(result);
    }
}
