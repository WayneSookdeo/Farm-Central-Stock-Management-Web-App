using Microsoft.AspNetCore.Mvc;

namespace FarmerCentralWebsite.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
