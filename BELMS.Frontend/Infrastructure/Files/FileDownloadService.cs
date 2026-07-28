using Microsoft.JSInterop;

namespace BELMS.Frontend.Infrastructure.Files;

public interface IFileDownloadService
{
    Task SaveAsync(string fileName, byte[] bytes, string contentType = ExcelContentType);

    const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
}

public sealed class FileDownloadService(IJSRuntime js) : IFileDownloadService
{
    public Task SaveAsync(string fileName, byte[] bytes, string contentType = IFileDownloadService.ExcelContentType) =>
        js.InvokeVoidAsync("belmsDownloadFile", fileName, contentType, Convert.ToBase64String(bytes)).AsTask();
}
