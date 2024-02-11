using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Backend.Models;
using Backend.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Backend.Controllers;

[Authorize]
public class ListItemController : Controller {
    private AppDbContext _db;

    public ListItemController(AppDbContext db) {
        _db = db;
    }

    public IActionResult Index() {
        string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdString, out int userId)) {
            var listItems = _db.ListItems
                .Where(listItem => listItem.UserId == userId)
                .ToList();
            return View(listItems);
        }
        else
            return RedirectToAction("Error");
    }

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

