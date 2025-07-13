using Microsoft.AspNetCore.Mvc;

namespace BloodDonationAPI.Controllers
{
    public class LegalController : Controller
    {
        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
