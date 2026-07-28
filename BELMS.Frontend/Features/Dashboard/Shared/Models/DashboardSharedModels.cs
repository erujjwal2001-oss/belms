namespace BELMS.Frontend.Features.Dashboard.Shared.Models;

public sealed record NotificationItem(
    string Title,
    string Message,
    DateTime Timestamp,
    bool IsUnread);

public sealed record WorkflowStep(
    string Label,
    string Status,
    bool IsComplete);

public sealed record TeamMember(
    string Name,
    string Role,
    string Status,
    string AvatarInitials);

public sealed record ApprovalItem(
    string Title,
    string Requestor,
    string Type,
    DateTime RequestedOn);

public sealed record AssetSummaryItem(
    string Category,
    int Total,
    int Available);

public sealed record AccessReviewItem(
    string User,
    string Resource,
    string Status,
    DateTime DueDate);
