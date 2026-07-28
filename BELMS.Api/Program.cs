using BELMS.Api.Extensions;
using BELMS.Application;
using BELMS.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("BelmsWeb", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173",
                "http://localhost:5185",
                "http://127.0.0.1:5185",
                "https://localhost:7142",
                "https://127.0.0.1:7142")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddOpenApi();

builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("BELMS API");
    });
}


app.ConfigureBelmsPipeline(); 
app.MapControllers();          

app.Run();