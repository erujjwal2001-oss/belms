using BELMS.Application.DTOs.Notifications;
using BELMS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BELMS.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationController(INotificationService notificationService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMyNotifications()
    {
        var result = await notificationService.GetMyNotificationsAsync();
        return ProcessResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await notificationService.GetByIdAsync(id);
        return ProcessResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request)
    {
        var result = await notificationService.CreateAsync(request);
        return ProcessResult(result);
    }

    [HttpPut("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var result = await notificationService.MarkAsReadAsync(id);
        return ProcessResult(result);
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var result = await notificationService.MarkAllAsReadAsync();
        return ProcessResult(result);
    }
}
