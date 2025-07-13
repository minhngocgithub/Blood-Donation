using Microsoft.AspNetCore.Mvc;

namespace BloodDonationAPI.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 