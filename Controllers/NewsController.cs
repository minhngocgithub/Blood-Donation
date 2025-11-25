using Microsoft.AspNetCore.Mvc;

namespace Blood_Donation_Website.Controllers
{
    public class NewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
