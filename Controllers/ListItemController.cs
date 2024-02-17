using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Models;
using Backend.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Backend.Controllers;

[Authorize]
public class ListItemController : Controller {
    private AppDbContext _db;
    private UserManager<User> _userManager;

    //Dealing with Database through DbContext
    public ListItemController(AppDbContext db, UserManager<User> userManager) {
        _db = db;
        _userManager = userManager;
    }

    //Fetching all List's Items based on the ID of the logged in user
    public IActionResult Index() {
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _userManager.FindByEmailAsync(userEmail).Result;
        if (user != null) {
            var listItems = _db.ListItems
                .Where(listItem => listItem.UserId == user.Id)
                .ToList();
            return View(listItems);
        }
        else
            return RedirectToAction("Error");
    }

    //Create a new list's item by an authorized user
    [HttpPost]
    public IActionResult Create(ListItem listItem) {
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _userManager.FindByEmailAsync(userEmail).Result;
        if (user != null) {
            listItem.UserId = user.Id;
            listItem.ListId = "1";
            _db.ListItems.Add(listItem);
            _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    //Update a specified list's item by its authorized user
    [HttpPost]
    public IActionResult Update(ListItem editedListItem) {
        var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _userManager.FindByEmailAsync(userEmail).Result;
        if (user != null) {
            var currentListItem = _db.ListItems.FirstOrDefault(listItem => listItem.Id == editedListItem.Id && listItem.UserId == user.Id);
            if (currentListItem != null) {
            currentListItem.Title = editedListItem.Title;
            currentListItem.Category = editedListItem.Category;
            currentListItem.Date = editedListItem.Date;
            currentListItem.Estimate = editedListItem.Estimate;
            currentListItem.Unit = editedListItem.Unit;
            currentListItem.Importance = editedListItem.Importance;
            currentListItem.ListId = editedListItem.ListId;
            _db.SaveChanges();
            }
        }
        return RedirectToAction("Index");
    }
}

