using Microsoft.Extensions.DependencyInjection;
using WebportSystem.Common.Application.Mail;

namespace WebportSystem.Common.Infrastructure.Mail;

internal static class MailExtension
{
    internal static IServiceCollection ConfigureMailing(this IServiceCollection services)
    {
        services.AddTransient<IMailService, MailService>();
        services.AddOptions<MailOptions>().BindConfiguration(nameof(MailOptions));

        return services;
    }
}