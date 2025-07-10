using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers
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
