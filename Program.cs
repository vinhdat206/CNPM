// File: Program.cs

using CNPMFastFood.Data;
using CNPMFastFood.Helpers;
using CNPMFastFood.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("SqliteConnection")));
// MVC
builder.Services.AddControllersWithViews();

// COOKIE AUTHENTICATION
builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Login";

        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;

        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = false;

        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async context =>
            {
                var appStartId =
                    context.Principal?.FindFirst("AppStartId")?.Value;

                if (appStartId != AppRuntime.AppStartId)
                {
                    context.RejectPrincipal();

                    await context.HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
        };
    })

    // GOOGLE LOGIN
    .AddGoogle(options =>
    {
        options.ClientId =
            builder.Configuration["Authentication:Google:ClientId"];

        options.ClientSecret =
            builder.Configuration["Authentication:Google:ClientSecret"];
    })

    //FACEBOOK LOGIN
    .AddFacebook(options =>
    {
        options.AppId =
            builder.Configuration["Authentication:Facebook:AppId"];

        options.AppSecret =
            builder.Configuration["Authentication:Facebook:AppSecret"];

        options.Scope.Add("email");
    });

// AUTHORIZATION
builder.Services.AddAuthorization();

// SESSION
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// SERVICES
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<SettingService>();
// Đăng ký CustomerService để Controller có thể sử dụng thông qua ICustomerService
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<SettingService>();

// VIETNAM CULTURE
var culture = new CultureInfo("vi-VN");

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

// BUILD APP
var app = builder.Build();

// MIDDLEWARE
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

// AREA ROUTE
app.MapControllerRoute(
    name: "areas",
    pattern:
        "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// DEFAULT ROUTE
app.MapControllerRoute(
    name: "default",
    pattern:
        "{controller=Home}/{action=Index}/{id?}");

// RUN
app.Run();