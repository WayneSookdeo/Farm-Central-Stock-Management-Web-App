using FarmerCentralWebsite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly FarmerCentralContext _dbContext;

    public AccountController()
    {
        _dbContext = new FarmerCentralContext();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if the user is a farmer
            var farmer = _dbContext.Farmers.FirstOrDefault(f => f.FarmerEmail == model.Email);
            if (farmer != null && farmer.Password == model.Password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, farmer.FarmerName),
                    new Claim(ClaimTypes.Email, farmer.FarmerEmail),
                    new Claim(ClaimTypes.Role, "Farmer")
                };

                await AuthenticateUser(claims);
                HttpContext.Session.SetString("FarmerEmail", model.Email);
                return RedirectToAction("Add", "Product"); // Redirect to the farmer's dashboard or relevant page
            }

            // Check if the user is an employee
            var employee = _dbContext.Employees.FirstOrDefault(e => e.EmployeeEmail == model.Email);
            if (employee != null && employee.Password == model.Password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, employee.EmployeeEmail),
                    new Claim(ClaimTypes.Email, employee.EmployeeEmail),
                    new Claim(ClaimTypes.Role, "Employee")
                };

                await AuthenticateUser(claims);

                return RedirectToAction("Add", "Farmer"); // Redirect to the employee's dashboard or relevant page
            }

            ModelState.AddModelError(string.Empty, "Invalid login credentials");
        }

        return View(model);
    }

    private async Task AuthenticateUser(List<Claim> claims)
    {
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties
            {
                IsPersistent = false // Change to true if you want persistent login
            });
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
