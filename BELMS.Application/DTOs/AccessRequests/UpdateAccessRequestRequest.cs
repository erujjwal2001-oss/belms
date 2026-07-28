using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.AccessRequests;

public class UpdateAccessRequestRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
