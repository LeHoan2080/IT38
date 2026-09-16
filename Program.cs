using AspNetMvcDemo.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

/**
* Configure authentication and authorization for the application.
* This allows the application to use cookie-based authentication and manage user access.
*/
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization();

/**
* Configure the database context for the application using SQL Server.
* The connection string is retrieved from the appsettings.json file.
*/
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

/**
* Add the ApplicationDbContext to the service container.
* This allows the application to use the database context for data access.
*/
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Serve static files from the wwwroot folder.
app.MapStaticAssets();

// Configure the default route for the application.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
