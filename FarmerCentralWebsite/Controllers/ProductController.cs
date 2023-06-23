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

    [Authorize(Roles = "Farmer")] // Requires authentication with "Farmer" role for accessing this controller and its actions
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [Authorize(Roles = "Farmer")] // Requires authentication with "Farmer" role for accessing this action
    [HttpPost]
    public IActionResult Add(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Create a new Product object with data from the model
            var product = new Product
            {
                ProductName = model.ProductName,
                ProductType = model.ProductType,
                StockPrice = model.StockPrice,
                DateSupplied = DateTime.Now // Set the current date and time as the supply date
            };

            // Add the product to the database and save changes
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            // Retrieve the logged-in farmer's email from the session
            string farmerEmail = HttpContext.Session.GetString("FarmerEmail");

            // Create a new Stock object associated with the product and the farmer's email
            var stock = new Stock
            {
                ProductId = product.ProductId,
                FarmerEmail = farmerEmail // Corrected lowercase "f" in "FarmerEmail"
            };

            // Add the stock to the database and save changes
            _dbContext.Stocks.Add(stock);
            _dbContext.SaveChanges();

            // Set a message to be displayed in the view
            ViewBag.Message = "Product added successfully.";

            return View();
        }

        return View(model);
    }

    [Authorize(Roles = "Employee")] // Requires authentication with "Employee" role for accessing this controller and its actions
    [HttpGet]
    public IActionResult Filter()
    {
        return View();
    }

    [Authorize(Roles = "Employee")] // Requires authentication with "Employee" role for accessing this action
    [HttpPost]
    public IActionResult Filter(FilterViewModel model)
    {
        if (ModelState.IsValid)
        {
            IQueryable<Product> query = _dbContext.Products.AsQueryable();

            // Apply filtering based on the provided criteria

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

            // Execute the query and retrieve the filtered products as a list
            List<Product> filteredProducts = query.ToList();

            // Return the "FilteredProducts" view with the filtered products
            return View("FilteredProducts", filteredProducts);
        }

        return View(model);
    }
}
