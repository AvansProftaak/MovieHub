using System.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieHub;
using MovieHub.Data;
using MovieHub.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Add google authentication//
var services = builder.Services;
var configuration = builder.Configuration;

services.AddAuthentication()
    
    .AddGoogle(googleOptions =>
    {
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
        facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
    })
    .AddTwitter(twitterOptions =>
    {
        twitterOptions.ConsumerKey = configuration["Authentication:Twitter:ConsumerAPIKey"];
        twitterOptions.ConsumerSecret = configuration["Authentication:Twitter:ConsumerSecret"];
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"];
        microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
    });   




var app = builder.Build();

var userManager = builder.Services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

SeedData.Seed(userManager!,roleManager!);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();