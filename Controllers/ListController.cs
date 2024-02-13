using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Models;
using Backend.Data;
using System.Security.Claims;

namespace Backend.Controllers;

[Authorize]
public class ListController : Controller {
    private AppDbContext _db;

    public ListController(AppDbContext db) {
        _db = db;
    }

    //Fetching all the lists from the database
    public IActionResult Index() {
        var lists = _db.Lists.ToList();
        return View(lists);
    }
}