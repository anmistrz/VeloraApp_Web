using ClassLibrary.DAL;
using ClassLibrary.DAL.DAL;
using ClassLibrary.DAL.Interfaces;
using DealerApi.DAL;
using DealerApi.DAL.Context;
using DealerApi.DAL.DAL;
using DealerApi.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace DealerApi.DAL.Extension
{
    public static class DataAccessLayerServiceExtension
    {
        public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DealerRndDBContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DealerRndDBConnectionString")));

            services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddRoles<IdentityRole>().AddEntityFrameworkStores<DealerRndDBContext>();


            // Register your data access layer services here
            services.AddScoped<ICustomer, CustomerDAL>();
            services.AddScoped<ICar, CarDAL>();
            services.AddScoped<IDealer, DealerDAL>();
            services.AddScoped<IDealerCar, DealerCarDAL>();
            services.AddScoped<IConsultHistory, ConsultHistoryDAL>();
            services.AddScoped<ITestDrive, TestDriveDAL>();
            services.AddScoped<IUserAuth, UserAuthDAL>();
            services.AddScoped<INotifications, NotificationDAL>();
            services.AddScoped<ISalesPerson, SalesPersonDAL>();
            services.AddScoped<IDealerCarUnit, DealerCarUnitDAL>();
            services.AddScoped<IEmailNotification, EmailNotificationDAL>();
            services.AddScoped<ISalesActivityLog, SalesActivityDAL>();
            // Add other services as needed

            return services;
        }
        
    }
}