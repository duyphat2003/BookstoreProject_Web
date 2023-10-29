﻿using BookstoreProject.Controllers;
using BookstoreProject.Firestore_Database;
using BookstoreProject.Models;
using Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddRoles<IdentityRole>()
//    .AddDefaultUI().
//    AddDefaultTokenProviders();

Encoding asciiEncoding = Encoding.ASCII;

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin",
        policy => policy.RequireRole("QuanLy", "ThuKho", "ThuThu"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o => o.LoginPath = new PathString("/user/signin"));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

Console.OutputEncoding = System.Text.Encoding.Unicode;
//Start Database ngoai nay, ha?n chê? go?i la?i trong t??ng controller
BookstoreProjectDatabase.ConnectToFirestoreDB();
CartList.CreateCart();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
