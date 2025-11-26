using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Services.Interfaces;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Utilities.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý tin tức (dành cho Admin và Hospital)
    /// Xử lý: CRUD tin tức, Xuất bản/Gỡ xuất bản, Xem công khai
    /// Route: /news/*
    /// </summary>
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: /News/Index
        [HospitalAdminOnly]
        public async Task<IActionResult> Index(string searchTerm = "", int? categoryId = null, bool? isPublished = null, string sortBy = "date")
        {
            return View();
        }
    }
}
