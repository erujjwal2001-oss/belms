using BELMS.Application.Interfaces.IService;
using BELMS.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BELMS.Infrastructure.Extensions;

public static class AuthenticationDependencyInjection
{
    public static IServiceCollection AddBelmsAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

        var jwtSettings = configuration
            .GetSection(JwtSettings.SectionName)
            .Get<JwtSettings>();

        if (jwtSettings is not null &&
            !string.IsNullOrWhiteSpace(jwtSettings.Key))
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnChallenge = context =>
                    //    {
                    //        context.HandleResponse();

                    //        context.Response.StatusCode = 401;
                    //        context.Response.ContentType = "application/json";

                    //        var response = new ApiResponse<object>
                    //        {
                    //            Success = false,
                    //            Message = "Unauthorized - token missing or invalid",
                    //            Code = "AUTH_401",
                    //            TraceId = context.HttpContext.TraceIdentifier
                    //        };

                    //        return context.Response.WriteAsJsonAsync(response);
                    //    },

                    //    OnForbidden = context =>
                    //    {
                    //        context.Response.StatusCode = 403;
                    //        context.Response.ContentType = "application/json";

                    //        var response = new ApiResponse<object>
                    //        {
                    //            Success = false,
                    //            Message = "Forbidden - access denied",
                    //            Code = "AUTH_403",
                    //            TraceId = context.HttpContext.TraceIdentifier
                    //        };

                    //        return context.Response.WriteAsJsonAsync(response);
                    //    }
                    //};
                });
        }

        services.AddAuthorization();

        return services;
    }
}