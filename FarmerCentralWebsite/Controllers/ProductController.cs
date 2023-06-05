using FarmerCentralWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


public class ProductController : Controller
{
    private readonly FarmerCentralContext _dbContext;

    public ProductController()
    {
        _dbContext = new FarmerCentralContext();
    }
    [Authorize(Roles = "Farmer")]

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    [Authorize(Roles = "Farmer")]
    // ProductController.cs

    [HttpPost]
    public IActionResult Add(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            var product = new Product
            {
                ProductName = model.ProductName,
                ProductType = model.ProductType,
                StockPrice = model.StockPrice,
                DateSupplied = DateTime.Now // or set your desired date here
            };

            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            // Retrieve the logged-in farmer's email
            string farmerEmail = HttpContext.Session.GetString("FarmerEmail");


            var stock = new Stock
            {
                ProductId = product.ProductId,
                FarmerEmail = farmerEmail // corrected lowercase "f"
            };

            _dbContext.Stocks.Add(stock);
            _dbContext.SaveChanges();

            ViewBag.Message = "Product added successfully.";

            return View();
        }

        return View(model);
    }

    [Authorize(Roles = "Employee")]
    [HttpGet]
    public IActionResult Filter()
    {
        return View();
    }
    [Authorize(Roles = "Employee")]
    [HttpPost]
    public IActionResult Filter(FilterViewModel model)
    {
        if (ModelState.IsValid)
        {
            IQueryable<Product> query = _dbContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(model.FarmerEmail))
            {
                query = query.Where(p => p.Stocks.Any(s => s.FarmerEmail == model.FarmerEmail));
            }

            if (model.StartDate.HasValue)
            {
                query = query.Where(p => p.DateSupplied >= model.StartDate.Value.Date);
            }

            if (model.EndDate.HasValue)
            {
                query = query.Where(p => p.DateSupplied <= model.EndDate.Value.Date);
            }

            if (!string.IsNullOrEmpty(model.ProductType))
            {
                query = query.Where(p => p.ProductType == model.ProductType);
            }

            List<Product> filteredProducts = query.ToList();

            return View("FilteredProducts", filteredProducts);
        }

        return View(model);
    }
}
