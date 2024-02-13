using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace Backend.Controllers;


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

    //Checking user authentication to redirect him/her to the Index page of listItems
    public IActionResult Login()
    {
        ClaimsPrincipal claimUser = HttpContext.User;
        if (claimUser.Identity.IsAuthenticated)
        {
            Console.WriteLine("HERE");
            return RedirectToAction("Index", "ListItem");
        }
        return View();
    }

    //Fetching user's data after logging in
    public IActionResult UserData()
    {
        string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdString, out int userId))
        {
            var fetchedUser = _db.Users
                .Where(user => user.Id == userId)
                .ToList();
            return View(fetchedUser);
        }
        else
            return RedirectToAction("Error");
    }

    //Reguest to login user and checking for his/her valid credentials through database
    [HttpPost]
    public async Task<IActionResult> Login(User userLogin) {

        var userInfo = _db.Users.SingleOrDefault(user => user.Email == userLogin.Email);
        if (userInfo != null && userLogin.Password == userInfo.Password) {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString())
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




    // public async Task<IActionResult> Login(User userLogin)
    // {
    //     var user = await _userManager.FindByEmailAsync(userLogin.Email);

    //     if (user != null && await _userManager.CheckPasswordAsync(user, userLogin.Password))
    //     {
    //         var claims = new List<Claim>
    //         {
    //             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    // 
    //         };

    //         var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    //         var authProperties = new AuthenticationProperties
    //         {
    //             AllowRefresh = true
    //         };

    //         await _signInManager.SignInAsync(user, authProperties);

    //         return RedirectToAction("Index", "ListItem");
    //     }

    //     ViewData["ValidateMessage"] = "Credentials Error";
    //     return View();
    // }
}