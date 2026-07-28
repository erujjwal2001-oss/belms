using System.ComponentModel.DataAnnotations;

namespace BELMS.Application.DTOs.AccessRequests;

public class CreateAccessRequestRequest
{
    [Required]
    public Guid EmployeeId { get; set; }

    [Required]
    public string RequestType { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
