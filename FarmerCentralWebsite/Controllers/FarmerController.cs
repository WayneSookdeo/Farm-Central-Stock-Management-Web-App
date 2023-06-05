using FarmerCentralWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[Authorize(Roles = "Employee")]
public class FarmerController : Controller
{
    private readonly FarmerCentralContext _dbContext;

    public FarmerController()
    {
        _dbContext = new FarmerCentralContext();
    }
    [Authorize(Roles = "Employee")]
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    [Authorize(Roles = "Employee")]
    [HttpPost]
    public IActionResult Add(FarmerViewModel model)
    {
        if (ModelState.IsValid)
        {
            var farmer = new Farmer
            {
                FarmerEmail = model.Email,
                FarmerName = model.Name,
                Password = model.Password
            };

            _dbContext.Farmers.Add(farmer);
            _dbContext.SaveChanges();

            ViewBag.Message = "Farmer added successfully.";

            return View();
        }

        return View(model);
    }
}
