using BELMS.Api.Middleware;

namespace BELMS.Api.Extensions;

public static class HostExtensions
{
    public static WebApplication ConfigureBelmsPipeline(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseCors("BelmsWeb");

        app.UseCorrelationId();

        app.UseExceptionHandling();


        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}