using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using WebportSystem.Common.Contracts.Identity;
using WebportSystem.Dashboard.Common.Authentication;
using WebportSystem.Dashboard.Common.HttpClients;
using WebportSystem.Dashboard.Common.Services;
using WebportSystem.Dashboard.Components;

var builder = WebApplication.CreateBuilder(args);

// Call this explicitly to enable assets in non-Development environments
StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// @CUSTOM-START

// Mudblazor 
builder.Services.AddMudServices();

// Http Clients
builder.Services.AddHttpClient<BaseHttpClient>((sp, client) =>
{
    IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(configuration["BaseUrls:Production"]!);
});

builder.Services.AddScoped<BaseHttpClient>();

// Services
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddScoped<UserContext>();

// Authentication
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PlatformOnly", policy =>
        policy.RequireAssertion(ctx =>
            !ctx.User.HasClaim(c => c.Type == CustomClaims.TenantId)));

    options.AddPolicy("TenantOnly", policy =>
        policy.RequireAssertion(ctx =>
            ctx.User.HasClaim(c => c.Type == CustomClaims.TenantId)));
});

// @CUSTOM-END

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AllowAnonymous(); // @CUSTOM

app.UseAuthorization();

await app.RunAsync();
