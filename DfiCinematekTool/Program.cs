using DfiCinematekTool.Components;
using DfiCinematekTool.Application;
using DfiCinematekTool.Infrastructure;
using DfiCinematekTool.Infrastructure.Identity;
using DfiCinematekTool.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using DfiCinematekTool.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
	options.Name = "DfiThemeCookie";
	options.Duration = TimeSpan.FromDays(365);

});

builder.Services.AddDbContext<CinematekDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CinematekConnection"));
});

builder.Services.AddSingleton<LoginStateService>();
builder.Services.AddScoped<ToasterService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequiredLength = 6;
	options.SignIn.RequireConfirmedEmail = false;

	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.AllowedForNewUsers = true;

	options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<CinematekDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.Name = "DfiAuth";
	options.Cookie.HttpOnly = true;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
	options.SlidingExpiration = true;
	options.LoginPath = "/Login";
	options.AccessDeniedPath = "/AccessDenied";
});

// Infrastructure dependencies
builder.Services.AddInfrastructure();

// Application dependencies
builder.Services.AddApplication();

var app = builder.Build();

// Seed Data
await app.Services.UseSeedUserAndRoleDataAsync();
await app.Services.UseSeedFilmsAndEventDataAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
