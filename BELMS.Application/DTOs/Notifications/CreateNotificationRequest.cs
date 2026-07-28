using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.Notifications;

public class CreateNotificationRequest
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Message { get; set; } = string.Empty;
}
