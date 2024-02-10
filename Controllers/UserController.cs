using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Backend.Controllers;

public class UserController : Controller {

    public IActionResult Login() {
        ClaimsPrincipal claimUser = HttpContext.User;
        if (claimUser.Identity.IsAuthenticated)
            return RedirectToAction("Index", "List");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(User userLogin) {

        if (userLogin.Email == "abed@gmail.com" && userLogin.Password == "abed0000") {
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
            return RedirectToAction("Index","List");
        }

        ViewData["ValidateMessage"] = "Credentials Error";
        return View();
    }
}