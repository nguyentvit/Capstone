using Capstone.Application;
using Capstone.Infrastructure;
using Capstone.API;
using Capstone.Infrastructure.Services.Identity;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Capstone.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var configuration = builder.Configuration;

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(configuration)
    .AddApiServices(configuration);

var app = builder.Build();

app.UseApiServices();

await app.InitializeDatabaseAsync();

InitializeDatabase(app);

app.Run();

static void InitializeDatabase(IApplicationBuilder app)
{
    using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope();

    serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

    var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
    context.Database.Migrate();

    context.Clients.RemoveRange(context.Clients);
    context.IdentityResources.RemoveRange(context.IdentityResources);
    context.ApiScopes.RemoveRange(context.ApiScopes);
    context.SaveChanges();

    if (!context.Clients.Any())
    {
        foreach (var client in IdentityConfig.Get)
        {
            context.Clients.Add(client.ToEntity());
        }
        context.SaveChanges();
    }

    if (!context.IdentityResources.Any())
    {
        foreach (var resource in IdentityConfig.IdentityResources)
        {
            context.IdentityResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }

    if (!context.ApiScopes.Any())
    {
        foreach (var resource in IdentityConfig.ApiScopes)
        {
            context.ApiScopes.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }
}

public partial class Program { }