namespace BELMS.Application.DTOs.AccessRequests;

public class AccessRequestDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Guid RequestedByUserId { get; set; }
    public string RequestedByName { get; set; } = string.Empty;
    public Guid? ApprovedByUserId { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
