using Blood_Donation_Website.Models;
using Blood_Donation_Website.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            // Lấy 3 sự kiện sắp tới
            var events = await _eventService.GetUpcomingEventsAsync();
            var top3Events = events.OrderBy(e => e.EventDate).ThenBy(e => e.StartTime).Take(3).ToList();
            return View(top3Events);
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