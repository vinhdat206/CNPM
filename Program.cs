// File: Program.cs
// Mô tả:
// Cấu hình hệ thống ASP.NET MVC

using CNPMFastFood.Data;
using CNPMFastFood.Services;

using Microsoft.EntityFrameworkCore;

// FIX FORMAT TIỀN VIỆT NAM
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// =========================
// SQL SERVER
// =========================

builder.Services.AddDbContext<AppDbContext>(
    options =>

        options.UseSqlServer(
            builder.Configuration
                .GetConnectionString(
                    "DefaultConnection"))
);

// =========================
// MVC
// =========================

builder.Services.AddControllersWithViews();

// =========================
// SESSION
// =========================

builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();

// =========================
// SERVICES
// =========================

// Product
builder.Services.AddScoped<ProductService>();

// Cart
builder.Services.AddScoped<CartService>();

// Order
builder.Services.AddScoped<OrderService>();

// Auth
builder.Services.AddScoped<AuthService>();

// =========================
// FORMAT TIỀN VIỆT NAM
// =========================

var culture = new CultureInfo("vi-VN");

CultureInfo.DefaultThreadCurrentCulture =
    culture;

CultureInfo.DefaultThreadCurrentUICulture =
    culture;

// =========================
// BUILD
// =========================

var app = builder.Build();

// =========================
// MIDDLEWARE
// =========================

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// =========================
// AREA ROUTE
// =========================

app.MapControllerRoute(
    name: "areas",
    pattern:
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// =========================
// DEFAULT ROUTE
// =========================

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Home}/{action=Index}/{id?}");

// =========================
// RUN
// =========================

app.Run();