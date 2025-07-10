using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 