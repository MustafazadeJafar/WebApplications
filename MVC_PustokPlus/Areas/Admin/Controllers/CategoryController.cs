﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_PustokPlus.Areas.Admin.ViewModels;
using MVC_PustokPlus.Contexts;
using MVC_PustokPlus.Models;

namespace MVC_PustokPlus.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin, Admin, Moderator")]
public class CategoryController : Controller
{
    public Pustoc02DbContext _db { get; set; }

    public CategoryController(Pustoc02DbContext db)
    {
        this._db = db;
    }

    // GET: CategoryController
    public async Task<IActionResult> Index()
    {
        return View(await _db.Categories.Select(c => new Category { Id = c.Id, Name = c.Name }).ToListAsync());
    }

    // GET: CategoryController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: CategoryController/Create
    public ActionResult Create()
    {
        ViewBag.Categories = _db.Categories;
        return View();
    }

    // POST: CategoryController/Create
    //[ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Create(AdminCategoryVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        if (await _db.Categories.AnyAsync(x => x.Name == vm.Name))
        {
            ModelState.AddModelError("Name", vm.Name + " already exist");
            return View(vm);
        }
        await _db.Categories.AddAsync(new Category { Name = vm.Name });
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: CategoryController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: CategoryController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: CategoryController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: CategoryController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
