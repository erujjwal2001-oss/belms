using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using BELMS.Frontend.Infrastructure.Authentication.Logout;
using BELMS.Frontend.Infrastructure.Authentication.Tokens;
using Microsoft.AspNetCore.Http;

namespace BELMS.Frontend.Infrastructure.Api
{
    public class ApiHandler
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LogoutService _logoutService;

        private readonly Dictionary<string, string> _headers = new();

        public ApiHandler(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, TokenService tokenService,LogoutService logoutService)
        {
            _tokenService = tokenService?? throw new ArgumentNullException(nameof(tokenService));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logoutService = logoutService;
        }

        public Dictionary<string, string> Headers => _headers;

        public async Task<HttpResponseMessage> SendRequest(HttpMethod method, string url, HttpContent? content = null, bool isAuth = true)
        {
            var request = new HttpRequestMessage(method, url);

            if (content != null)
            {
                request.Content = content;
            }

            if (isAuth)
            {
                var token = await _tokenService.GetAccessTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    await _logoutService.LogoutAsync();
                    throw new UnauthorizedAccessException(
                        "Authentication expired.");
                }
                request.Headers.Authorization =new AuthenticationHeaderValue("Bearer",token);
                ApplyHeaders(request);
            }
            return await _httpClient.SendAsync(request);
        }

        private void ApplyHeaders(HttpRequestMessage request)
        {
            foreach (var header in _headers)
            {
                if (request.Content != null && IsContentHeader(header.Key))
                {
                    request.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                else
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

        }

        private bool IsContentHeader(string key)
        {
            return key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase) ||
                   key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase) ||
                   key.StartsWith("Content-", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<TResponse?> GetAsync<TResponse>(string url, bool isAuth = true)
        {
            var response = await SendRequest(HttpMethod.Get, url, null, isAuth);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data, bool isAuth = true)
        {
            var content = JsonContent.Create(data);
            var response = await SendRequest(HttpMethod.Post, url, content, isAuth);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data, bool isAuth = true)
        {
            var content = JsonContent.Create(data);
            var response = await SendRequest(HttpMethod.Put, url, content, isAuth);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task<TResponse?> DeleteAsync<TResponse>(string url, bool isAuth = true)
        {
            var response = await SendRequest(HttpMethod.Delete, url, null, isAuth);
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
    }
}
