using BELMS.Frontend.Infrastructure.Api.Contracts;

namespace BELMS.Frontend.Infrastructure.Api.Clients;

public interface IEmployeeApiClient
{
    Task<ApiResult<PagedResponse<EmployeeDto>>> GetAllAsync(string? search, int page, int pageSize);

    Task<ApiResult<EmployeeDto>> GetByIdAsync(Guid id);

    Task<ApiResult<EmployeeDto>> CreateAsync(CreateEmployeeRequest request);

    Task<ApiResult<EmployeeDto>> UpdateAsync(Guid id, UpdateEmployeeRequest request);

    Task<ApiResult> DeleteAsync(Guid id);

    Task<ApiResult<byte[]>> ExportAsync();

    Task<ApiResult<EmployeeImportResult>> ImportAsync(Stream stream, string fileName);
}

public sealed class EmployeeImportResult
{
    public int SuccessCount { get; set; }

    public int FailureCount { get; set; }

    public List<string> Errors { get; set; } = [];
}

public sealed class EmployeeApiClient(ApiHandler api) : ApiClientBase(api), IEmployeeApiClient
{
    public Task<ApiResult<PagedResponse<EmployeeDto>>> GetAllAsync(string? search, int page, int pageSize)
    {
        var url = $"{ApiEndpoints.Employees}?PageNumber={page}&PageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
        {
            url += $"&Search={Uri.EscapeDataString(search)}";
        }

        return GetAsync<PagedResponse<EmployeeDto>>(url);
    }

    public Task<ApiResult<EmployeeDto>> GetByIdAsync(Guid id) =>
        GetAsync<EmployeeDto>(ApiEndpoints.Employee(id));

    public Task<ApiResult<EmployeeDto>> CreateAsync(CreateEmployeeRequest request) =>
        PostAsync<EmployeeDto>(ApiEndpoints.Employees, request);

    public Task<ApiResult<EmployeeDto>> UpdateAsync(Guid id, UpdateEmployeeRequest request) =>
        PutAsync<EmployeeDto>(ApiEndpoints.Employee(id), request);

    public Task<ApiResult> DeleteAsync(Guid id) =>
        DeleteAsync(ApiEndpoints.Employee(id));

    public Task<ApiResult<byte[]>> ExportAsync() =>
        DownloadAsync(ApiEndpoints.ExportEmployees);

    public Task<ApiResult<EmployeeImportResult>> ImportAsync(Stream stream, string fileName) =>
        UploadAsync<EmployeeImportResult>(ApiEndpoints.ImportEmployees, stream, fileName);
}
