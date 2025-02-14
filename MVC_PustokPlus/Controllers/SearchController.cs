﻿using Microsoft.AspNetCore.Mvc;
using MVC_PustokPlus.Contexts;
using MVC_PustokPlus.Helpers;
using MVC_PustokPlus.Models;
using MVC_PustokPlus.ViewModels;
using System.Drawing;
using System.Security.Policy;

namespace MVC_PustokPlus.Controllers;

public class SearchController : Controller
{
    Pustoc02DbContext _db { get; }

    public int ItemsPerPage { get; set; } = 1;

    public SearchController(Pustoc02DbContext db)
    {
        this._db = db;
    }

    public async Task<IActionResult> Index(string? q, IEnumerable<string>? categories, string? sortedBy)
    {
        int count = _db.Products.Count(p => p.IsDeleted == false);
        string attributes = "&";

        var items = _db.Products.Where(p => p.IsDeleted == false).Take((int)ItemsPerPage).Select(p => new ProductSliderVM
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Discount = p.Discount,
            SellPrice = (p.SellPrice * (100 - (decimal)p.Discount) / 100).ToString("0.00"),
            Category = p.Category,
            CostPrice = p.SellPrice.ToString("0.00"),
            Count = p.Count,
            FrontImagePath = p.FrontImagePath,
            BackImagePath = p.BackImagePath,
            ProductImages = p.ProductImages,
        });

        return View(new Pagination<IEnumerable<ProductSliderVM>>(items, 1, ItemsPerPage, count, "Search", nameof(ProductTable), attributes));
    }
    
    public async Task<IActionResult> ProductTable(int page, int ipp)
	{
        var querryResult = this._db.Products.Select(p => !p.IsDeleted);



        int count = _db.Products.Count(p => p.IsDeleted == false);
        int from = (page - 1) * ipp;

        var items = _db.Products.Where(p => p.IsDeleted == false).Skip(from).Take((int)ipp).Select(p => new ProductSliderVM
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Discount = p.Discount,
            SellPrice = (p.SellPrice * (100 - (decimal)p.Discount) / 100).ToString("0.00"),
            Category = p.Category,
            CostPrice = p.SellPrice.ToString("0.00"),
            Count = p.Count,
            FrontImagePath = p.FrontImagePath,
            BackImagePath = p.BackImagePath,
            ProductImages = p.ProductImages,
        });

        return PartialView("_ProductTablePartial", 
            new Pagination<IEnumerable<ProductSliderVM>>(items, page, ipp, count, "Search", nameof(ProductTable), querry));
    }
}
