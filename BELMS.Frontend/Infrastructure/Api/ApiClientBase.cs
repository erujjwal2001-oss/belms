using System.Net.Http.Headers;
using System.Net.Http.Json;
using BELMS.Frontend.Models;

namespace BELMS.Frontend.Infrastructure.Api;

/// <summary>
/// Base for typed API clients. Wraps <see cref="ApiHandler"/>, unwraps the standard
/// <c>ApiResponse&lt;T&gt;</c> envelope, and normalizes transport/auth failures into <see cref="ApiResult{T}"/>.
/// </summary>
public abstract class ApiClientBase(ApiHandler api)
{
    protected ApiHandler Api { get; } = api;

    protected async Task<ApiResult<T>> GetAsync<T>(string url)
    {
        try
        {
            var response = await Api.SendRequest(HttpMethod.Get, url);
            return await ReadAsync<T>(response);
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Fail(ex.Message);
        }
    }

    protected async Task<ApiResult<T>> PostAsync<T>(string url, object body)
    {
        try
        {
            var response = await Api.SendRequest(HttpMethod.Post, url, JsonContent.Create(body));
            return await ReadAsync<T>(response);
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Fail(ex.Message);
        }
    }

    protected async Task<ApiResult<T>> PutAsync<T>(string url, object body)
    {
        try
        {
            var response = await Api.SendRequest(HttpMethod.Put, url, JsonContent.Create(body));
            return await ReadAsync<T>(response);
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Fail(ex.Message);
        }
    }

    protected async Task<ApiResult> PostAsync(string url, object? body = null)
    {
        try
        {
            var content = body is null ? null : JsonContent.Create(body);
            var response = await Api.SendRequest(HttpMethod.Post, url, content);
            return await ReadAsync(response);
        }
        catch (Exception ex)
        {
            return ApiResult.Fail(ex.Message);
        }
    }

    protected async Task<ApiResult> PutAsync(string url, object? body = null)
    {
        try
        {
            var content = body is null ? null : JsonContent.Create(body);
            var response = await Api.SendRequest(HttpMethod.Put, url, content);
            return await ReadAsync(response);
        }
        catch (Exception ex)
        {
            return ApiResult.Fail(ex.Message);
        }
    }

    protected async Task<ApiResult> DeleteAsync(string url)
    {
        try
        {
            var response = await Api.SendRequest(HttpMethod.Delete, url);
            return await ReadAsync(response);
        }
        catch (Exception ex)
        {
            return ApiResult.Fail(ex.Message);
        }
    }

    /// <summary>Downloads a raw binary payload (e.g. an Excel export).</summary>
    protected async Task<ApiResult<byte[]>> DownloadAsync(string url)
    {
        try
        {
            var response = await Api.SendRequest(HttpMethod.Get, url);
            if (!response.IsSuccessStatusCode)
            {
                return ApiResult<byte[]>.Fail($"Download failed ({(int)response.StatusCode}).");
            }

            var bytes = await response.Content.ReadAsByteArrayAsync();
            return ApiResult<byte[]>.Ok(bytes);
        }
        catch (Exception ex)
        {
            return ApiResult<byte[]>.Fail(ex.Message);
        }
    }

    /// <summary>Uploads a stream as multipart/form-data under the field name <c>file</c>.</summary>
    protected async Task<ApiResult<T>> UploadAsync<T>(string url, Stream stream, string fileName)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(fileContent, "file", fileName);

            var response = await Api.SendRequest(HttpMethod.Post, url, content);
            return await ReadAsync<T>(response);
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Fail(ex.Message);
        }
    }

    private static async Task<ApiResult<T>> ReadAsync<T>(HttpResponseMessage response)
    {
        var payload = await SafeReadAsync<T>(response);
        if (payload is { IsSuccess: true })
        {
            return ApiResult<T>.Ok(payload.Data);
        }

        return ApiResult<T>.Fail(payload?.Message ?? DefaultError(response));
    }

    private static async Task<ApiResult> ReadAsync(HttpResponseMessage response)
    {
        var payload = await SafeReadAsync<object>(response);
        if (payload is { IsSuccess: true } || response.IsSuccessStatusCode)
        {
            return ApiResult.Ok();
        }

        return ApiResult.Fail(payload?.Message ?? DefaultError(response));
    }

    private static async Task<ApiResponse<T>?> SafeReadAsync<T>(HttpResponseMessage response)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<ApiResponse<T>>();
        }
        catch
        {
            return null;
        }
    }

    private static string DefaultError(HttpResponseMessage response) =>
        $"Request failed with status {(int)response.StatusCode} ({response.ReasonPhrase}).";
}
