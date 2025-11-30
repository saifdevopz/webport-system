using Scalar.AspNetCore;
using System.Reflection;
using System.Security.Claims;
using WebportSystem.Api.Extensions;
using WebportSystem.Common.Application;
using WebportSystem.Common.Infrastructure;
using WebportSystem.Common.Presentation.Endpoints;
using WebportSystem.Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- Database Configuration ---
var config = builder.Configuration;

string activeProvider = config["Database:ActiveProvider"]
    ?? throw new ArgumentException("Missing Database:ActiveProvider in configuration.");

string basePath = $"Database:Providers:{activeProvider}";

// Fetch connection strings dynamically
string? identityDbString = config[$"{basePath}:IdentityConnection"];

ArgumentException.ThrowIfNullOrWhiteSpace(identityDbString);

// --- MVC & API ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// --- Global Exception Handling ---
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
    context.ProblemDetails.Instance =
        $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
});

// --- Application Modules ---
Assembly[] moduleApplicationAssemblies =
[
    WebportSystem.Identity.Application.AssemblyReference.Assembly
];

// --- Common Modules ---
builder.Services.AddCommonApplication(moduleApplicationAssemblies);
builder.Services.AddCommonInfrastructure(builder.Configuration);

// --- Infrastructure Modules ---
builder.Services.AddIdentityModule(builder.Configuration, identityDbString);

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("MyPolicy");

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference(_ =>
    {
        _.Servers = [];
        _.Theme = ScalarTheme.Kepler;
    });

    await app.InitializeDatabases();
}

app.UseHttpsRedirection();

// Converts unhandled exceptions into Problem Details responses
app.UseExceptionHandler();

// Returns the Problem Details response for (empty) non-successful responses
app.UseStatusCodePages();

app.MapGet("me", (HttpContext httpContext, ClaimsPrincipal claimsPrincipal) =>
{
    var request = httpContext.Request;

    // Check if Authorization header contains a Bearer token
    var authHeader = request.Headers["Authorization"].FirstOrDefault();
    var hasBearerToken = authHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ?? false;

    return Results.Ok(new
    {
        User = claimsPrincipal.Identity?.Name,
        Claims = claimsPrincipal.Claims.Select(c => new { c.Type, c.Value }),
        RequestInfo = new
        {
            Method = request.Method,
            Path = request.Path.ToString(),
            Query = request.Query.ToDictionary(q => q.Key, q => q.Value.ToString()),
            Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()),
            HasBearerToken = hasBearerToken,
            BearerToken = hasBearerToken ? authHeader : null
        }
    });
}).RequireAuthorization();




app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapEndpoints();

await app.RunAsync();
