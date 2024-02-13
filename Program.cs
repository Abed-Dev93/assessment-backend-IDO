using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Backend.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

//Connecting to the database through the string DEFAULT CONNECTION
builder.Services.AddDbContext<AppDbContext>(options => {
options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
new MariaDbServerVersion(new Version(10, 5, 0)));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowLocalhost3000", builder => {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

//Identifying user ID after logging in
builder.Services.AddIdentity<User, Backend.Models.IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


//Initializing cookie for authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(option =>
{
    option.LoginPath = "/User/Login";
    option.ExpireTimeSpan = TimeSpan.FromHours(12);
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
app.UseCors("AllowLocalhost3000");

app.UseAuthentication();

app.UseAuthorization();

//Default route
app.MapControllerRoute(
    name: "default/Login",
    pattern: "{controller=User}/{action=Login}/{id?}");

//Logout route
app.MapControllerRoute(
    name: "Logout",
    pattern: "{controller=User}/{action=Logout}/{id?}");

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
    pattern: "{controller=ListItem}/{action=CreateListItem}/{id?}");

//ListItem update route
app.MapControllerRoute(
    name: "ListItemEdition",
    pattern: "{controller=ListItem}/{action=UpdateListItem}/{id?}");

app.Run("http://localhost:4000");
