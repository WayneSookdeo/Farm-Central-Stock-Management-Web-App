using FarmerCentralWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[Authorize(Roles = "Employee")] // Requires authentication with "Employee" role for accessing this controller and its actions
public class FarmerController : Controller
{
    private readonly FarmerCentralContext _dbContext;

    public FarmerController()
    {
        _dbContext = new FarmerCentralContext();
    }

    [Authorize(Roles = "Employee")] // Requires authentication with "Employee" role for accessing this action
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [Authorize(Roles = "Employee")] // Requires authentication with "Employee" role for accessing this action
    [HttpPost]
    public IActionResult Add(FarmerViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Create a new Farmer object with data from the model
            var farmer = new Farmer
            {
                FarmerEmail = model.Email,
                FarmerName = model.Name,
                Password = model.Password
            };

            // Add the farmer to the database and save changes
            _dbContext.Farmers.Add(farmer);
            _dbContext.SaveChanges();

            // Set a message to be displayed in the view
            ViewBag.Message = "Farmer added successfully.";

            return View();
        }

        return View(model);
    }
}
