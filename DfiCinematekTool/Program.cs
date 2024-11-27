using DfiCinematekTool.Components;
using DfiCinematekTool.Application;
using DfiCinematekTool.Application.Interfaces;
using DfiCinematekTool.Application.Services;
using DfiCinematekTool.Infrastructure;
using DfiCinematekTool.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using DfiCinematekTool.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<CinematekDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CinematekConnection"));
});

// Infrastructure dependencies
builder.Services.AddInfrastructure();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;

}).AddEntityFrameworkStores<CinematekDbContext>();

// Application dependencies
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();
app.UseAuthentication();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
