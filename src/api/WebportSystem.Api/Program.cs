using Scalar.AspNetCore;
using System.Reflection;
using WebportSystem.Common.Application;
using WebportSystem.Common.Infrastructure;
using WebportSystem.Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// --- Database Configuration (Dynamic, Multi-Cloud Support) ---
var config = builder.Configuration;

string provider = config["Database:ActiveProvider"]
    ?? throw new ArgumentException("Missing Database:ActiveProvider in configuration.");

string basePath = $"Database:Providers:{provider}";

// Fetch connection strings dynamically
string? identityDbString = config[$"{basePath}:IdentityConnection"];

// Optional fallback for Aspire local development
if (string.IsNullOrWhiteSpace(identityDbString))
{
    var localDevConnection = config.GetConnectionString("demo-db");
    if (!string.IsNullOrWhiteSpace(localDevConnection))
    {
        identityDbString ??= localDevConnection;
    }
}

ArgumentException.ThrowIfNullOrWhiteSpace(identityDbString);

// --- MVC & API ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

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
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
