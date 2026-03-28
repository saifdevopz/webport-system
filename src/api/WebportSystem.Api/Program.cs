using QuestPDF.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;
using WebportSystem.Api.Extensions;
using WebportSystem.Common.Application;
using WebportSystem.Common.Infrastructure;
using WebportSystem.Common.Presentation.Endpoints;
using WebportSystem.Identity.Domain.Roles;
using WebportSystem.Identity.Infrastructure;
using WebportSystem.Identity.Infrastructure.Database;
using WebportSystem.Inventory.Domain.Entities.Category;
using WebportSystem.Inventory.Infrastructure;
using WebportSystem.Inventory.Infrastructure.Database;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// --- Logging ---
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

IWebHostEnvironment environment = builder.Environment;
Log.Information("Running in {EnvironmentName} environment.", environment.EnvironmentName);

// --- Third Party Configurations ---
QuestPDF.Settings.License = LicenseType.Community;

// --- Database Configurations ---
ConfigurationManager config = builder.Configuration;

string activeProvider = config["Database:ActiveProvider"]
    ?? throw new ArgumentException("Missing Database:ActiveProvider in configuration.");

string basePath = $"Database:Providers:{activeProvider}";

string identityDbString = config[$"{basePath}:IdentityConnection"]
    ?? throw new ArgumentException("Missing IdentityConnection in configuration.");

string inventoryDbString = config[$"{basePath}:InventoryConnection"]
    ?? throw new ArgumentException("Missing InventoryConnection in configuration.");

// --- API ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// --- Error Handling ---
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = context =>
{
    context.ProblemDetails.Instance =
        $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
});

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

// --- Application Modules ---
Assembly[] moduleApplicationAssemblies =
[
    WebportSystem.Identity.Application.AssemblyReference.Assembly,
    WebportSystem.Inventory.Application.AssemblyReference.Assembly
];

builder.Services.AddCommonApplication(
    moduleApplicationAssemblies,
    moduleContexts: [
        (typeof(UsersDbContext), typeof(RoleM).Assembly),
        (typeof(InventoryDbContext), typeof(CategoryM).Assembly),
    ]);

builder.Services.AddCommonInfrastructure(builder.Configuration);

// --- Infrastructure Modules ---
builder.Services.AddIdentityModule(builder.Configuration, identityDbString);
builder.Services.AddInventoryModule(builder.Configuration, inventoryDbString);

var app = builder.Build();

// --- Endpoints ---
app.MapControllers();
app.MapEndpoints();

// --- Infrastructure ---
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("MyPolicy");

// --- Observability ---
app.MapHealthChecks("/health");
app.UseSerilogRequestLogging();

// --- Error Handling ---
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseApplicationMiddlewares();

// --- Authentication & Authorization ---
app.UseAuthentication();
app.UseAuthorization();

// --- API Documentation ---
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.MapOpenApi();
    app.MapScalarApiReference("/", _ =>
    {
        _.WithTitle("My Blueprint");
        _.Servers = [];
        _.Theme = ScalarTheme.Kepler;
    });
}

// --- Database Initialization ---
if (!app.Environment.IsDevelopment())
{
    DatabaseInitializer.InitializeDatabases(app).Wait();
}

await app.RunAsync();
