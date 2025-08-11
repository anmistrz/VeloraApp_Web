using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DealerApi.DAL.Extension;
using WebPromotion.Services.Consultation;
// using WebPromotion.BL.DealerCar;
using WebPromotion.Services.DealerCar;
using WebPromotion.Business;
using WebPromotion.Business.Interface;
using WebPromotion.Services;
using WebPromotion.Services.TestDrive;
using WebPromotion.Services.Interface;
using WebPromotion.Middleware;
using WebPromotion.Helpers;
using DealerApi.Application.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<WebPromotion.Filters.SetUserRoleFilter>();
});

// Configure Auth
builder.Services.AddAuthentication(defaultScheme: IdentityConstants.ApplicationScheme);
    // .AddIdentityCookies();

// Add Identity Core services
builder.Services.AddIdentityCore<IdentityUser>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

// Register DAL and Application services
builder.Services.AddDataAccessLayerServices(builder.Configuration);

//Make auto set Bearer Token
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddHttpClient("ApiWithBearer")
    .AddHttpMessageHandler<BearerTokenHandler>();

// Register Application services
builder.Services.AddHttpClient<IDealerCarServices, DealerCarServices>();
builder.Services.AddHttpClient<IConsultationServices, ConsultationServices>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
});
builder.Services.AddHttpClient<ITestDriveService, TestDriveServices>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
});
builder.Services.AddHttpClient<IAccountServices, AccountService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<BearerTokenHandler>();
builder.Services.AddHttpClient<INotificationServices, NotificationServices>()
    .AddHttpMessageHandler<BearerTokenHandler>();


builder.Services.AddScoped<IDealerCarBusiness, DealerCarbusiness>();
builder.Services.AddScoped<IConsultationBusiness, ConsultationBusiness>();
builder.Services.AddScoped<ITestDriveBusiness, TestDriveBusiness>();
builder.Services.AddScoped<IAccountBusiness, AccountBusiness>();
builder.Services.AddScoped<INotificationBusiness, NotificationBusiness>();  

// Register MVC services
builder.Services.AddScoped<IConsultationServices, ConsultationServices>();

builder.Services.AddApplicationServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<TokenExpiryRedirectMiddleware>();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Error/NotFound");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
