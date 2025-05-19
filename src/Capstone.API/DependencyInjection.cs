using BuildingBlocks.Exceptions.Handler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Capstone.API;
public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuth(configuration);

        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyConstant.ADMIN, policy => policy.RequireRole(RoleConstant.ADMIN))
            .AddPolicy(PolicyConstant.TEACHER, policy => policy.RequireRole(RoleConstant.TEACHER))
            .AddPolicy(PolicyConstant.STUDENT, policy => policy.RequireRole(RoleConstant.STUDENT))
            .AddPolicy(PolicyConstant.ADMIN_OR_TEACHER, policy => policy.RequireRole(RoleConstant.ADMIN, RoleConstant.TEACHER));

        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
        });

        services.AddHttpContextAccessor();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.WithOrigins("http://localhost:5173")
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });
        });

        services.AddCarter();

        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }
    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseExceptionHandler(options => { });

        app.UseRouting();

        app.UseCors("AllowAllOrigins");

        app.UseIdentityServer();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapCarter();

        return app;
    }
    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var environment = configuration["ASPNETCORE_ENVIRONMENT"];

        if (environment == "Development")
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.Authority = "http://localhost:5036";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        return services;
    }
}