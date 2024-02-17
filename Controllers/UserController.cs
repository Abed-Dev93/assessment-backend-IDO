using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using BCrypt.Net;
using Microsoft.AspNetCore.Cors;

namespace Backend.Controllers;


[ApiController]
[Route("User")]
[EnableCors()]
public class UserController : Controller
{
    private AppDbContext _db;
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;

    //Dealing with Database through DbContext
    public UserController(AppDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _db = db;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Register() {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(User model) {
        if (ModelState.IsValid) {
            var user = new User { UserName = model.Email, Email = model.Email };
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            user.PasswordHash = hashedPassword;
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded) {
                await _signInManager.SignInAsync(user, new AuthenticationProperties{ IsPersistent = false });
                return RedirectToAction("Login", "User");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
        return View(model);
    }

    [HttpGet("Login")]
    //Checking user authentication to redirect him/her to the Index page of listItems
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "ListItem");
        return View();
    }

    //Reguest to login user and checking for his/her valid credentials through database
    [HttpPost]
    public async Task<IActionResult> Login(User userLogin) {

        var user = await _userManager.FindByEmailAsync(userLogin.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, userLogin.Password)) {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Email)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties() {
                AllowRefresh = true
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);
            return RedirectToAction("Index", "List");
        }

        ViewData["ValidateMessage"] = "Credentials Error";
        return View();
    }

    //Fetching user's data after logging in
    public IActionResult UserData()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userEmail != null) {
            var fetchedUser = _userManager.FindByEmailAsync(userEmail).Result;
            if (fetchedUser != null)
                return View(fetchedUser);
        }
        return RedirectToAction("Error");
    }

}