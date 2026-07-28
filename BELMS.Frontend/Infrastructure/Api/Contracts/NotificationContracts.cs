using System.ComponentModel.DataAnnotations;

namespace BELMS.Frontend.Infrastructure.Api.Contracts;

public sealed class NotificationDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTime? ReadAt { get; set; }

    public DateTime CreatedAt { get; set; }
}

public sealed class CreateNotificationRequest
{
    [Required(ErrorMessage = "User is required.")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Message is required.")]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;
}
