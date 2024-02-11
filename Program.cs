using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(option => {
    option.LoginPath = "/User/Login";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(1);
});

builder.Services.AddDbContext<AppDbContext>(options => 
options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
new MariaDbServerVersion(new Version(10, 5, 0))));

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "List",
    pattern: "{Controller=List}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "ListItem",
    pattern: "{controller=ListItem}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "ListItemCreation",
    pattern: "{controller=ListItem}/{action=CreateListItem}/{id?}");

app.MapControllerRoute(
    name: "ListItemEdition",
    pattern: "{controller=ListItem}/{action=UpdateListItem}/{id?}");

app.Run("http://localhost:4000");
