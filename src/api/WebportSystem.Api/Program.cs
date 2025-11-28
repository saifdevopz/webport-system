using Scalar.AspNetCore;
using System.Reflection;
using WebportSystem.Api.Extensions;
using WebportSystem.Common.Application;
using WebportSystem.Common.Infrastructure;
using WebportSystem.Common.Presentation.Endpoints;
using WebportSystem.Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
//from work
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

// --- Exception Handling ---
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

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

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapEndpoints();

await app.RunAsync();
