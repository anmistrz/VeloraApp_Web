using DealerApi.Application.Interface;
using DealerApi.Application.BusinessLogic;
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

        // // DEBUG: Print all configuration key-value pairs
        // Console.WriteLine("==== CONFIGURATION DUMP ====");
        // foreach (var kvp in configuration.AsEnumerable())
        // {
        //     Console.WriteLine($"{kvp.Key} = {kvp.Value}");
        // }
        // Console.WriteLine("===========================");

        //add jwt token
        var appSettingsSection = configuration.GetSection("AppSettings");
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

        services.AddScoped<ICarBL, CarBL>();
        services.AddScoped<IDealerBL, DealerBL>();
        services.AddScoped<IDealerCarBL, DealerCarBL>();
        services.AddScoped<IConsultHistoryBL, ConsultHistoryBL>();
        services.AddScoped<ITestDriveBL, TestDriveBL>();
        services.AddScoped<IUserAuthBL, UserAuthBL>();
        services.AddScoped<INotificationBL, NotificationBL>();
        services.AddScoped<ISalesPersonBL, SalesPersonBL>();
        services.AddScoped<IDealerCarUnitBL, DealerCarUnitBL>();
        services.AddScoped<IEmailNotificationBL, EmailNotificationBL>();
        services.AddScoped<ISalesActivityLogBL, SalesActivityBL>();
        // Add other services as needed

        // Removed recursive call to AddApplicationServices()

        return services;
    }
}