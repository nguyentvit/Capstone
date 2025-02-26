using System.Net;
using System.Net.Mail;
using Capstone.Application.Interface.Common.Services;
using Capstone.Domain.Identity.Models;
using Capstone.Infrastructure.Data;
using Capstone.Infrastructure.Data.Interceptors;
using Capstone.Infrastructure.DTO;
using Capstone.Infrastructure.Services.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Capstone.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) => 
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

        services.Configure<DataProtectionTokenProviderOptions>(options => {
            options.TokenLifespan = TimeSpan.FromDays(1);
        });

        services.AddIdentity<ApplicationUser, IdentityRole>(options => {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedAccount = true;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddEmailSender(configuration);
        services.AddScoped<IEmailSender, EmailSender>();
        return services;
    }
    private static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpSettings = new SmtpSettings();
        configuration.Bind(SmtpSettings.SectionName, smtpSettings);
        services.AddSingleton(Options.Create(smtpSettings));

        var smtpClient = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
        {
            Credentials = new NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
            EnableSsl = true
        };

        ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

        services.AddFluentEmail(smtpSettings.FromEmail, smtpSettings.FromName)
            .AddSmtpSender(smtpClient)
            .AddRazorRenderer();

        return services;
    }
}