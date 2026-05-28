// File: Program.cs

using CNPMFastFood.Data;
using CNPMFastFood.Helpers;
using CNPMFastFood.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// SQLITE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// MVC
builder.Services.AddControllersWithViews();

// COOKIE AUTHENTICATION
// COOKIE AUTHENTICATION
var authBuilder = builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
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

                if (!string.IsNullOrEmpty(appStartId) &&
                    appStartId != AppRuntime.AppStartId)
                {
                    context.RejectPrincipal();

                    await context.HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
        };
    })
    .AddCookie("External");

var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

if (!string.IsNullOrWhiteSpace(googleClientId) &&
    !string.IsNullOrWhiteSpace(googleClientSecret))
{
    authBuilder.AddGoogle("Google", options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
        options.SignInScheme = "External";
        options.CallbackPath = "/signin-google";
    });
}

var facebookAppId = builder.Configuration["Authentication:Facebook:AppId"];
var facebookAppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];

if (!string.IsNullOrWhiteSpace(facebookAppId) &&
    !string.IsNullOrWhiteSpace(facebookAppSecret))
{
    authBuilder.AddFacebook("Facebook", options =>
    {
        options.AppId = facebookAppId;
        options.AppSecret = facebookAppSecret;
        options.SignInScheme = "External";
        options.CallbackPath = "/signin-facebook";

        options.Scope.Add("email");
        options.Fields.Add("name");
        options.Fields.Add("email");
    });
}
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


Console.WriteLine("CONTENT ROOT: " + app.Environment.ContentRootPath);
Console.WriteLine("DB FULL PATH: " + Path.GetFullPath("food.db"));
Console.WriteLine("DB EXISTS: " + File.Exists("food.db"));

// CREATE DEFAULT ADMIN
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Nếu chưa có admin
    if (!db.Users.Any(x => x.Role == "admin"))
    {
        var hashedPassword =
            BCrypt.Net.BCrypt.HashPassword("@Admin123");

        db.Users.Add(new CNPMFastFood.Models.User
        {
            Username = "admin",
            Email = "adminescfood@gmail.com",

            Password = hashedPassword,
            ConfirmPassword = hashedPassword,

            Role = "admin",
            IsBlocked = false
        });

        db.SaveChanges();

        Console.WriteLine("DEFAULT ADMIN CREATED");
    }

    Console.WriteLine("PRODUCT COUNT: " + db.Products.Count());
}

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