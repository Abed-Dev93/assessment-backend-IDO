using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Backend.Controllers;

public class UserController : Controller {

    private AppDbContext _db;

    public UserController(AppDbContext db) {
        _db = db;
    }

    public IActionResult Login() {
        ClaimsPrincipal claimUser = HttpContext.User;
        if (claimUser.Identity.IsAuthenticated)
            return RedirectToAction("Index", "ListItem");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(User userLogin) {

        var userInfo = _db.Users.SingleOrDefault(user => user.Email == userLogin.Email);
        if (userInfo != null && userLogin.Password == userInfo.Password) {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, userLogin.Email)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties() {
                AllowRefresh = true
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);
            return RedirectToAction("Index", "ListItem");
        }

        ViewData["ValidateMessage"] = "Credentials Error";
        return View();
    }
}