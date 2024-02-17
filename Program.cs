using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Backend.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//Connecting to the database through the string DEFAULT CONNECTION
builder.Services.AddDbContext<AppDbContext>(options => {
options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
new MariaDbServerVersion(new Version(10, 5, 0)));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(builder => {
        builder.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

//Identifying user ID after logging in
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


//Initializing cookie for authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(option =>
{
    option.LoginPath = "/User/Login";
    option.AccessDeniedPath = "/User/Login";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(1);
});

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
app.UseCors();

app.UseAuthentication();

app.UseAuthorization();


// //Register route
// app.MapControllerRoute(
//     name: "Register",
//     pattern: "{controller=User}/{action=Register}/{id?}");

//Default route
app.MapControllerRoute(
    name: "User",
    pattern: "{controller=User}/{action=Login}/{id?}");

//User fetch route
app.MapControllerRoute(
    name: "User",
    pattern: "{controller=User}/{action=UserData}/{id?}");

//List fetch route
app.MapControllerRoute(
    name: "List",
    pattern: "{Controller=List}/{action=Index}/{id?}");

//ListItem fetch route
app.MapControllerRoute(
    name: "ListItem",
    pattern: "{controller=ListItem}/{action=Index}/{id?}");

//ListItem creation route
app.MapControllerRoute(
    name: "ListItemCreation",
    pattern: "{controller=ListItem}/{action=Create}/{id?}");

//ListItem update route
app.MapControllerRoute(
    name: "ListItemEdition",
    pattern: "{controller=ListItem}/{action=Update}/{id?}");

//Logout route
app.MapControllerRoute(
    name: "Logout",
    pattern: "{controller=User}/{action=Logout}/{id?}");

app.Run("http://localhost:4000");
