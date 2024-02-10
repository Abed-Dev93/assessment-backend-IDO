using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers;

public class ListItemController : Controller {
    private AppDbContext _db;

    public ListItemController(AppDbContext db) {
        _db = db;
    }

    public IActionResult Index() {
        var listItems = _db.ListItems.ToList();
        return View(listItems);
    }

    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public IActionResult CreateList(ListItem listItem) {
        _db.ListItems.Add(listItem);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}