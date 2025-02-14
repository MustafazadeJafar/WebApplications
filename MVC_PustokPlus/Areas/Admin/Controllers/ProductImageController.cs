﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_PustokPlus.Areas.Admin.ViewModels;
using MVC_PustokPlus.Contexts;
using MVC_PustokPlus.Helpers;
using MVC_PustokPlus.Models;

namespace MVC_PustokPlus.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin, Admin, Moderator")]
public class ProductImageController : Controller
{
    Pustoc02DbContext _db { get; }

    public ProductImageController(Pustoc02DbContext db)
    {
        this._db = db;
    }
    // GET: ProductImageController
    public ActionResult Index()
    {
        return View(_db.Products.Where(p2 => p2.IsDeleted == false).Select(p => new AdminProductImageListVM
        {
            ProductName = p.Name,
            CustomImages = p.ProductImages.Select(pi => new AdminProductImageListVM.CustomImage
            {
                Id = pi.Id,
                ImagePath = pi.ImagePath,
            }).ToList(),
        }));
    }

    // GET: ProductImageController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: ProductImageController/Create
    public ActionResult Create()
    {
        ViewBag.Products = new SelectList(_db.Products, "Id", "Name");
        return View();
    }

    // POST: ProductImageController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(AdminProductImageCreateVM vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _db.Categories;
            return View(vm);
        }


        ProductImage prodImg;
        foreach (var item in vm.Images)
        {
            if (item != null)
            {
                if (!item.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!item.IsValidSize(70000))
                {
                    ModelState.AddModelError("ImageFile", "Files length must be less than kb");
                }

                prodImg = new ProductImage()
                {
                    ProductId = vm.ProductId,
                    ImagePath = item.SaveAsync("datas").Result
                };
                await _db.ProductImages.AddAsync(prodImg);
            }
        }

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: ProductImageController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: ProductImageController/Edit/5
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

    // GET: ProductImageController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: ProductImageController/Delete/5
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
