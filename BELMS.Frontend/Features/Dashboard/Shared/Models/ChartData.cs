namespace BELMS.Frontend.Features.Dashboard.Shared.Models;

public sealed record ChartData(
    string Title,
    IReadOnlyList<string> Labels,
    IReadOnlyList<double> Values)
{
    public int SegmentCount => Math.Min(Labels?.Count ?? 0, Values?.Count ?? 0);
}
