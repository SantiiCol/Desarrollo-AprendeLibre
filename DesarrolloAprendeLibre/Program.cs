using DesarrolloAprendeLibre.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Cookies para el login
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Acceso/Index"; //Nuestro formulario de login
        option.ExpireTimeSpan = TimeSpan.FromMinutes(30); //Tiempo de vida del logueo
        option.AccessDeniedPath = "/Home/Privacy"; //Formulario de Acceso denegado
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EstudiantePolicy", policy => policy.RequireRole("Estudiante"));
    options.AddPolicy("ProfesorPolicy", policy => policy.RequireRole("Profesor"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Move the AddDbContext line before building the app
builder.Services.AddDbContext<AplDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie HTTP only
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

builder.Services.AddControllersWithViews();

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

app.UseRouting();

app.UseSession(); // Enable session middleware

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Registrar}/{id?}");

app.Run();

