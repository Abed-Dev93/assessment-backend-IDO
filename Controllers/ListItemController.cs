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
        string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine("USER>>>>>>>", userIdString);
        if (int.TryParse(userIdString, out int userId)) {
            var listItems = _db.ListItems
                .Where(listItem => listItem.UserId == userId)
                .ToList();
            return View(listItems);
        }
        else
            return RedirectToAction("Error");
    }

    //Create a new list's item by an authorized user
    [HttpPost]
    public IActionResult CreateListItem(ListItem listItem) {
        string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdString, out int userId)) {
            listItem.UserId = userId;
            listItem.ListId = 1;
            _db.ListItems.Add(listItem);
            _db.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    //Update a specified list's item by its authorized user
    [HttpPost]
    public IActionResult UpdateListItem(ListItem editedListItem) {
        string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdString, out int userId)) {
            var currentListItem = _db.ListItems.SingleOrDefault(listItem => listItem.Id == editedListItem.Id && listItem.UserId == userId);
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

