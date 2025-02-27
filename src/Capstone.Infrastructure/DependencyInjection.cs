using System.Net;
using System.Net.Mail;
using Capstone.Application.Interface;
using Capstone.Application.Interface.Common.Services;
using Capstone.Application.Interface.Services.Identity;
using Capstone.Domain.Identity.Models;
using Capstone.Infrastructure.Data;
using Capstone.Infrastructure.Data.Interceptors;
using Capstone.Infrastructure.DTO;
using Capstone.Infrastructure.Services.Common;
using Capstone.Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

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

        services.AddEmailSender(configuration);
        services.AddIdentity(configuration);
        services.AddRedis(configuration);
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
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
    private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {   
        
        var environment = configuration["ASPNETCORE_ENVIRONMENT"];
        var connection = configuration.GetConnectionString("Database");
        var migrationsAssembly = typeof(DependencyInjection).Assembly.GetName().FullName;

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Database"));
        });

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(15);
        });

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {

                // configure password
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

                // configure signin
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedAccount = true;
                
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddIdentityServer(options =>
        {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;

            })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = db =>
                        db.UseSqlServer(connection,
                        sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = db =>
                    db.UseSqlServer(connection,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddDeveloperSigningCredential()
                ;

        return services;
    } 
    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfiguration = configuration.GetConnectionString("Redis");
        var options = ConfigurationOptions.Parse(redisConfiguration!);
        options.AbortOnConnectFail = false;
        var connectionMultiplexer = ConnectionMultiplexer.Connect(options);
        services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfiguration;
        });
        services.AddScoped<ISendOtpService, SendOtpService>();
        return services;
    }
}