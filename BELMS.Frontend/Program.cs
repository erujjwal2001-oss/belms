using BELMS.Frontend.App;
using BELMS.Frontend.Features.Authentication.Services;
using BELMS.Frontend.Features.Dashboard.Services;
using BELMS.Frontend.Infrastructure.Api;
using BELMS.Frontend.Infrastructure.Authentication;
using BELMS.Frontend.Models;
using Microsoft.AspNetCore.Authentication;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

#region Configuration

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? throw new InvalidOperationException("ApiBaseUrl is not configured.");

#endregion

#region Services

builder.AddServiceDefaults();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.Cookie.Name = "MyApp.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/access-denied";
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Authentication infrastructure
builder.Services.AddBelmsAuthentication(builder.Configuration, apiBaseUrl);

// HTTP clients
builder.Services.AddHttpClient<ApiHandler>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Typed API clients over ApiHandler
builder.Services.AddBelmsApiClients();

// Feature services
builder.Services.AddBelmsDashboardServices();

// UI
builder.Services.AddMudServices();

#endregion

var app = builder.Build();

#region Startup

app.LogTokenCacheBackend();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/not-found",createScopeForStatusCodePages: true);

app.UseAntiforgery();

app.MapStaticAssets();

#endregion

#region Endpoints
app.MapDefaultEndpoints();
app.MapGet("/auth/logout", async (
    HttpContext context,
    IFAuthenticationService authenticationService) =>
{
    await authenticationService.LogoutAsync();
    await context.SignOutAsync("Cookies");
    context.Response.Redirect("/login");
});
app.MapGet("/auth/login", async (
    string email,
    string password,
    HttpContext context,
    IFAuthenticationService authenticationService) =>
{
    var result = await authenticationService.LoginAsync(
        new LoginRequest
        {
            Email = email,
            Password = password
        });

    if (!result.Success || result.Principal == null)
    {
        return Results.Redirect("/login?error=invalid");
    }

    await context.SignInAsync(
        "Cookies",
        result.Principal,
        new AuthenticationProperties
        {
            IsPersistent= true
        });

    return Results.Redirect("/dashboard");
});



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

#endregion

app.Run();