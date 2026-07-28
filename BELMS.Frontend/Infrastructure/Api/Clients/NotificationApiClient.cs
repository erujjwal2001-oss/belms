using BELMS.Frontend.Infrastructure.Api.Contracts;

namespace BELMS.Frontend.Infrastructure.Api.Clients;

public interface INotificationApiClient
{
    Task<ApiResult<List<NotificationDto>>> GetMyNotificationsAsync();

    Task<ApiResult<NotificationDto>> GetByIdAsync(Guid id);

    Task<ApiResult> MarkAsReadAsync(Guid id);

    Task<ApiResult> MarkAllAsReadAsync();

    Task<ApiResult<NotificationDto>> CreateAsync(CreateNotificationRequest request);
}

public sealed class NotificationApiClient(ApiHandler api) : ApiClientBase(api), INotificationApiClient
{
    public Task<ApiResult<List<NotificationDto>>> GetMyNotificationsAsync() =>
        GetAsync<List<NotificationDto>>(ApiEndpoints.Notifications);

    public Task<ApiResult<NotificationDto>> GetByIdAsync(Guid id) =>
        GetAsync<NotificationDto>(ApiEndpoints.Notification(id));

    public Task<ApiResult> MarkAsReadAsync(Guid id) =>
        PutAsync(ApiEndpoints.MarkNotificationRead(id));

    public Task<ApiResult> MarkAllAsReadAsync() =>
        PutAsync(ApiEndpoints.MarkAllNotificationsRead);

    public Task<ApiResult<NotificationDto>> CreateAsync(CreateNotificationRequest request) =>
        PostAsync<NotificationDto>(ApiEndpoints.Notifications, request);
}
