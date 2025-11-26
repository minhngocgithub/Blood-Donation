using Microsoft.AspNetCore.Mvc;
using Blood_Donation_Website.Models.DTOs;
using Blood_Donation_Website.Services.Interfaces;
using static Blood_Donation_Website.Utilities.EnumMapper;
using Blood_Donation_Website.Models.ViewModels;

namespace Blood_Donation_Website.Controllers
{
    /// <summary>
    /// Controller quản lý sự kiện hiến máu (dành cho người dùng)
    /// Xử lý: Xem danh sách sự kiện, Chi tiết sự kiện, Đăng ký tham gia
    /// Route: /events/*
    /// </summary>
    public class EventsController : Controller
    {
        // Các service dependencies để xử lý logic nghiệp vụ
        private readonly IBloodDonationEventService _eventService; // Service quản lý sự kiện
        private readonly IDonationRegistrationService _registrationService; // Service quản lý đăng ký
        private readonly IDonationHistoryService _donationHistoryService; // Service quản lý lịch sử hiến máu
        private readonly IBloodTypeService _bloodTypeService; // Service quản lý nhóm máu

        /// <summary>
        /// Constructor - Inject các service cần thiết
        /// </summary>
        public EventsController(IBloodDonationEventService eventService, IDonationRegistrationService registrationService, IDonationHistoryService donationHistoryService, IBloodTypeService bloodTypeService)
        {
            _eventService = eventService;
            _registrationService = registrationService;
            _donationHistoryService = donationHistoryService;
            _bloodTypeService = bloodTypeService;
        }

        /// <summary>
        /// GET: /Events/Index
        /// Hiển thị danh sách tất cả sự kiện hiến máu (có tìm kiếm và lọc)
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm (tên sự kiện, mô tả)</param>
        /// <param name="location">Lọc theo địa điểm</param>
        /// <param name="fromDate">Lọc từ ngày</param>
        /// <param name="toDate">Lọc đến ngày</param>
        /// <param name="bloodType">Lọc theo nhóm máu cần hiến</param>
        public async Task<IActionResult> Index(string searchTerm, string location, DateTime? fromDate, DateTime? toDate, string? bloodType)
        {
            try
            {
                // Lấy tất cả nhóm máu để hiển thị trong dropdown lọc
                var bloodTypes = await _bloodTypeService.GetAllBloodTypesAsync();
                ViewBag.BloodTypes = bloodTypes;

                // Tạo DTO tìm kiếm với các bộ lọc
                var searchDto = new EventSearchDto
                {
                    SearchTerm = searchTerm,
                    FromDate = fromDate,
                    ToDate = toDate,
                    RequiredBloodTypes = bloodType,
                    Page = 1,
                    PageSize = 50, // Lấy nhiều sự kiện để nhóm lại
                    SortBy = "EventDate",
                    SortOrder = "asc"
                };

                var allEvents = await _eventService.GetEventsPagedAsync(searchDto);
                var currentDate = DateTime.Now.Date;

                // Phân nhóm sự kiện thành sự kiện hiện tại/sắp tới và sự kiện đã qua
                var currentAndUpcomingEvents = allEvents.Items
                    .Where(e => e.EventDate >= currentDate) // Sự kiện từ hôm nay trở đi
                    .OrderBy(e => e.EventDate) // Sắp xếp theo ngày tăng dần
                    .ToList();

                var pastEvents = allEvents.Items
                    .Where(e => e.EventDate < currentDate) // Sự kiện đã qua
                    .OrderByDescending(e => e.EventDate) // Sắp xếp theo ngày giảm dần
                    .ToList();

                // Tạo ViewModel để truyền dữ liệu sang View
                var viewModel = new EventsViewModel
                {
                    CurrentAndUpcomingEvents = currentAndUpcomingEvents,
                    PastEvents = pastEvents,
                    SearchTerm = searchTerm,
                    Location = location,
                    FromDate = fromDate,
                    ToDate = toDate,
                    SelectedBloodType = bloodType,
                    TotalCurrentEvents = currentAndUpcomingEvents.Count,
                    TotalPastEvents = pastEvents.Count
                };

                return View(viewModel);
            }
            catch
            {
                // Nếu có lỗi, trả về danh sách rỗng
                return View(new EventsViewModel
                {
                    CurrentAndUpcomingEvents = new List<BloodDonationEventDto>(),
                    PastEvents = new List<BloodDonationEventDto>()
                });
            }
        }

        /// <summary>
        /// GET: /Events/LoadMoreEvents
        /// Load thêm sự kiện (dùng cho pagination/infinite scroll)
        /// </summary>
        /// <param name="page">Trang cần load</param>
        /// <param name="pageSize">Số sự kiện mỗi trang</param>
        /// <param name="eventType">Loại sự kiện: "current" (sắp tới) hoặc "past" (đã qua)</param>
        [HttpGet]
        public async Task<IActionResult> LoadMoreEvents(int page = 1, int pageSize = 6, string? eventType = "current")
        {
            try
            {
                var searchDto = new EventSearchDto
                {
                    Page = page,
                    PageSize = pageSize,
                    SortBy = "EventDate",
                    SortOrder = eventType == "past" ? "desc" : "asc" // Sự kiện quá khứ: mới nhất trước
                };

                var pagedResult = await _eventService.GetEventsPagedAsync(searchDto);
                var currentDate = DateTime.Now.Date;

                // Lọc sự kiện dựa trên loại
                var filteredEvents = eventType == "past" 
                    ? pagedResult.Items.Where(e => e.EventDate < currentDate).ToList() // Sự kiện đã qua
                    : pagedResult.Items.Where(e => e.EventDate >= currentDate).ToList(); // Sự kiện sắp tới
                
                // Nếu là AJAX request, trả về partial view
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_EventCards", filteredEvents);
                }
                
                // Ngược lại trả về JSON
                return Json(filteredEvents);
            }
            catch
            {
                return Json(new List<BloodDonationEventDto>());
            }
        }

        /// <summary>
        /// GET: /Events/Details/{id}
        /// Hiển thị chi tiết sự kiện hiến máu
        /// Kiểm tra xem người dùng đã đăng ký chưa và thời gian đủ điều kiện hiến máu
        /// </summary>
        /// <param name="id">ID sự kiện</param>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var eventDetails = await _eventService.GetEventByIdAsync(id);
                if (eventDetails == null)
                {
                    return NotFound();
                }
                
                DonationRegistrationDto? userRegistration = null;
                
                // Nếu người dùng đã đăng nhập, kiểm tra thông tin đăng ký và điều kiện hiến máu
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    {
                        // Kiểm tra xem người dùng đã đăng ký sự kiện này chưa
                        userRegistration = await _registrationService.GetUserRegistrationForEventAsync(userId, id);
                        
                        // Lấy ngày có thể hiến máu tiếp theo (sau 90 ngày từ lần hiến trước)
                        var nextEligibleDate = await _donationHistoryService.GetUserNextEligibleDateAsync(userId);
                        int? daysLeft = null;
                        string eligibleMessage = string.Empty;
                        
                        if (nextEligibleDate.HasValue)
                        {
                            var now = DateTime.Now.Date;
                            if (now >= nextEligibleDate.Value.Date)
                            {
                                eligibleMessage = "Bạn đã có thể hiến máu.";
                            }
                            else
                            {
                                daysLeft = (nextEligibleDate.Value.Date - now).Days;
                                eligibleMessage = $"Bạn cần chờ {daysLeft} ngày nữa để hiến máu.";
                            }
                        }
                        else
                        {
                            // Chưa hiến máu lần nào hoặc đã đủ điều kiện
                            eligibleMessage = "Bạn đã có thể hiến máu.";
                        }
                        
                        // Truyền dữ liệu sang View
                        ViewBag.NextEligibleDate = nextEligibleDate;
                        ViewBag.DaysLeft = daysLeft;
                        ViewBag.EligibleMessage = eligibleMessage;
                    }
                }
                ViewBag.UserRegistration = userRegistration;
                return View(eventDetails);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// POST: /Events/Register
        /// Đăng ký tham gia sự kiện hiến máu
        /// Chỉ người dùng đã đăng nhập mới có thể đăng ký
        /// </summary>
        /// <param name="eventId">ID sự kiện cần đăng ký</param>
        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo vệ chống CSRF attack
        public async Task<IActionResult> Register(int eventId)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "Bạn cần đăng nhập để đăng ký sự kiện.";
                return RedirectToAction("Details", new { id = eventId });
            }

            // Lấy UserId từ Claims
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                TempData["Error"] = "Không xác định được người dùng.";
                return RedirectToAction("Details", new { id = eventId });
            }

            try
            {
                // Tạo DTO đăng ký mới
                var createDto = new DonationRegistrationCreateDto
                {
                    UserId = userId,
                    EventId = eventId,
                    Notes = null // Ghi chú có thể thêm sau
                };
                
                // Gọi service để tạo đăng ký
                var result = await _registrationService.CreateRegistrationAsync(createDto);
                
                // Kiểm tra kết quả đăng ký
                if (result != null && result.Status == RegistrationStatus.Confirmed)
                {
                    TempData["Success"] = "Đăng ký sự kiện thành công!";
                }
                else
                {
                    // Hiển thị lý do thất bại nếu có
                    TempData["Error"] = result?.CancellationReason ?? "Đăng ký thất bại hoặc bạn đã đăng ký sự kiện này.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            
            // Quay lại trang chi tiết sự kiện
            return RedirectToAction("Details", new { id = eventId });
        }
    }
}
