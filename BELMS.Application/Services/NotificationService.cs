using AutoMapper;
using BELMS.Application.DTOs.Notifications;
using BELMS.Application.Interfaces.IRepo;
using BELMS.Application.Interfaces.IService;
using BELMS.Domain.Common;
using BELMS.Domain.Common.Constants;
using BELMS.Domain.Entities;
using BELMS.Domain.Enums;

namespace BELMS.Application.Services;

public class NotificationService(
    INotificationRepository notificationRepository,
    IUserRepository userRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : INotificationService
{
    public async Task<Result<NotificationDto>> CreateAsync(CreateNotificationRequest request)
    {
        // Admin-created notification for a specific user
        var notification = new Notification
        {
            UserId = request.UserId,
            Title = request.Title,
            Message = request.Message,
            IsRead = false
        };

        await notificationRepository.AddAsync(notification);
        await notificationRepository.SaveChangesAsync();

        return Result<NotificationDto>.Success(mapper.Map<NotificationDto>(notification));
    }

    public async Task<Result<NotificationDto>> GetByIdAsync(Guid id)
    {
        var notification = await notificationRepository.GetByIdAsync(id);
        if (notification is null)
        {
            return Result<NotificationDto>.Failure(
                Error.NotFound("Notification.NotFound", NotificationMessages.NotFound));
        }

        // Users may only read their own notifications
        if (notification.UserId != currentUserService.UserId)
        {
            return Result<NotificationDto>.Failure(
                Error.Forbidden("Notification.Forbidden", NotificationMessages.NotFound));
        }

        return Result<NotificationDto>.Success(mapper.Map<NotificationDto>(notification));
    }

    public async Task<Result<List<NotificationDto>>> GetMyNotificationsAsync()
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId is null)
        {
            return Result<List<NotificationDto>>.Failure(
                Error.Unauthorized("Auth.Unauthorized", AuthMessages.Unauthorized));
        }

        // Newest notifications first (ordered in repository)
        var notifications = await notificationRepository.GetByUserIdAsync(currentUserService.UserId.Value);
        return Result<List<NotificationDto>>.Success(mapper.Map<List<NotificationDto>>(notifications));
    }

    public async Task<Result> MarkAsReadAsync(Guid id)
    {
        var notification = await notificationRepository.GetByIdAsync(id);
        if (notification is null || notification.UserId != currentUserService.UserId)
        {
            return Result.Failure(Error.NotFound("Notification.NotFound", NotificationMessages.NotFound));
        }

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await notificationRepository.UpdateAsync(notification);
        await notificationRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> MarkAllAsReadAsync()
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId is null)
        {
            return Result.Failure(Error.Unauthorized("Auth.Unauthorized", AuthMessages.Unauthorized));
        }

        // Bulk update all unread notifications for current user
        var notifications = await notificationRepository.GetByUserIdAsync(currentUserService.UserId.Value);
        foreach (var notification in notifications.Where(x => !x.IsRead))
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await notificationRepository.UpdateAsync(notification);
        }

        await notificationRepository.SaveChangesAsync();
        return Result.Success();
    }

    public async Task NotifyUsersByRoleAsync(string role, string title, string message)
    {
        // Skip silently when role string is not a valid enum
        if (!Enum.TryParse<Role>(role, true, out var parsedRole))
        {
            return;
        }

        // Fan-out: one notification row per user with matching role
        var users = await userRepository.GetByRoleAsync(parsedRole);
        foreach (var user in users)
        {
            await notificationRepository.AddAsync(new Notification
            {
                UserId = user.Id,
                Title = title,
                Message = message,
                IsRead = false
            });
        }

        await notificationRepository.SaveChangesAsync();
    }
}
