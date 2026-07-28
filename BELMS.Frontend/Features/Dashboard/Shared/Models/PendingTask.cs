namespace BELMS.Frontend.Features.Dashboard.Shared.Models;

public sealed record PendingTask(
    string Title,
    string Status,
    string Assignee,
    DateTime DueDate);
