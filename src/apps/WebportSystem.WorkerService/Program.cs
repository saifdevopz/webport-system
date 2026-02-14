using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebportSystem.WorkerService;

var builder = WebApplication.CreateBuilder(args);

#pragma warning disable S1075 // URIs should not be hardcoded
builder.WebHost.UseUrls("http://localhost:5055");
#pragma warning restore S1075 // URIs should not be hardcoded

builder.Services.AddHostedService<Worker>();
builder.Services.AddWindowsService();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAngular");

// Test endpoint
app.MapGet("/ping", () =>
{
    return Results.Ok(new
    {
        status = "online",
        version = "1.0.0"
    });
});


await app.RunAsync();