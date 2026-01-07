using QuestPDF.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;
using WebportSystem.Api.Extensions;
using WebportSystem.Common.Application;
using WebportSystem.Common.Infrastructure;
using WebportSystem.Common.Presentation.Endpoints;
using WebportSystem.Identity.Infrastructure;
using WebportSystem.Inventory.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Build logger temporarily so we can log before app is built
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

// Environment info logging
IWebHostEnvironment environment = builder.Environment;

if (environment.IsDevelopment())
{
    Log.Information("Running in Development environment.");
}
else if (environment.IsProduction())
{
    Log.Information("Running in Production environment.");
}
else
{
    Log.Information("Running in {EnvironmentName} environment.", environment.EnvironmentName);
}

// --- QuestPDF ---
QuestPDF.Settings.License = LicenseType.Community;

// --- Database Configurations ---
ConfigurationManager config = builder.Configuration;

string activeProvider = config["Database:ActiveProvider"]
    ?? throw new ArgumentException("Missing Database:ActiveProvider in configuration.");

string basePath = $"Database:Providers:{activeProvider}";

// Fetch connection strings dynamically
string? identityDbString = config[$"{basePath}:IdentityConnection"];
string? inventoryDbString = config[$"{basePath}:InventoryConnection"];

ArgumentException.ThrowIfNullOrWhiteSpace(identityDbString);
ArgumentException.ThrowIfNullOrWhiteSpace(inventoryDbString);

// --- Serilog ---
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

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
    WebportSystem.Identity.Application.AssemblyReference.Assembly,
    WebportSystem.Inventory.Application.AssemblyReference.Assembly
];

// --- Common Modules ---
builder.Services.AddCommonApplication(moduleApplicationAssemblies);
builder.Services.AddCommonInfrastructure(builder.Configuration);

// --- Infrastructure Modules ---
builder.Services.AddIdentityModule(builder.Configuration, identityDbString);
builder.Services.AddInventoryModule(builder.Configuration, inventoryDbString);

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

// Health Checks

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseCors("MyPolicy");

app.UseStaticFiles();

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

app.UseApplicationMiddlewares();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

// Converts unhandled exceptions into Problem Details responses
app.UseExceptionHandler();

// Returns the Problem Details response for (empty) non-successful responses
app.UseStatusCodePages();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapEndpoints();

await app.RunAsync();
