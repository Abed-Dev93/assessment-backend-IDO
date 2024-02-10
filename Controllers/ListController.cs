using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Data;

namespace Backend.Controllers;

public class ListController : Controller {
    private AppDbContext _db;

    public ListController(AppDbContext db) {
        _db = db;
    }

    public IActionResult Index() {
        var lists = _db.Lists.ToList();
        return View(lists);
    }

    public IActionResult Create() {
        return View();
    }

    [HttpPost]
    public IActionResult CreateList(List list) {
        _db.Lists.Add(list);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}