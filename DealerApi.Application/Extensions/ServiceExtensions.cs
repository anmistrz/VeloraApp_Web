using DealerApi.Application.Interface;
using DealerApi.Application.Services;
using DealerApi.DAL.Extension;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using DealerApi.Application.Helpers;
using Microsoft.Extensions.Configuration; // Add this for Get<T> extension

namespace DealerApi.Application.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register your application services here
        services.AddDataAccessLayerServices(configuration);

        //add jwt token
        var appSettingsSection = services.BuildServiceProvider()
            .GetRequiredService<IConfiguration>()
            .GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);
        var appSettings = appSettingsSection.Get<AppSettings>();
        if (string.IsNullOrEmpty(appSettings?.Secret))
        {
            throw new ArgumentNullException(nameof(appSettings.Secret), "AppSettings Secret cannot be null or empty");
        }

        var key = Encoding.ASCII.GetBytes(appSettings.Secret);
        if (key.Length == 0)
        {
            throw new ArgumentException("AppSettings Secret must be a valid non-empty string", nameof(appSettings.Secret));
        }

        services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddScoped<ICarServices, CarServices>();
        services.AddScoped<IDealerService, DealerServices>();
        services.AddScoped<IDealerCarServices, DealerCarServices>();
        services.AddScoped<IConsultHistoryServices, ConsultHistoryServices>();
        services.AddScoped<ITestDriveServices, TestDriveServices>();
        services.AddScoped<IUserAuthServices, UserAuthServices>();
        services.AddScoped<INotificationServices, NotificationServices>();
        services.AddScoped<ISalesPersonServices, SalesPersonServices>();
        services.AddScoped<IDealerCarUnitServices, DealerCarUnitServices>();
        // Add other services as needed

        // Removed recursive call to AddApplicationServices()

        return services;
    }
}