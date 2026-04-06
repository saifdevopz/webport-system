using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebportSystem.Common.Infrastructure.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationInternal(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<TenantContext>();

        services.AddOptions<JwtOptions>()
            .BindConfiguration("JwtOptions")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddAuthentication(authentication =>
            {
                authentication.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                JwtOptions jwtOptions = services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>().Value;

                options.RequireHttpsMetadata = true;
                options.SaveToken = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.FromMinutes(jwtOptions.ClockSkewInMinutes)
                };
            });

        return services;
    }
}
