using Capstone.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Capstone.Infrastructure.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

        await context.Database.MigrateAsync();

        await seeder.SeedAsync();
    }
}