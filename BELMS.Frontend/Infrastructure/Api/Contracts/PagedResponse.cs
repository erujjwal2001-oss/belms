namespace BELMS.Frontend.Infrastructure.Api.Contracts;

public sealed class PagedResponse<T>
{
    public List<T> Items { get; set; } = [];

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages { get; set; }
}
