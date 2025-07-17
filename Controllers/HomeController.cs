using Blood_Donation_Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Models.DTOs;
using System.Threading.Tasks;
using System.Linq;

namespace Blood_Donation_Website.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IBloodDonationEventService _eventService;

        public HomeController(IBloodDonationEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            var upcomingEvents = (await _eventService.GetUpcomingEventsAsync()).Take(3).ToList();
            return View(upcomingEvents);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Guide()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        public IActionResult SweetAlertDemo()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}