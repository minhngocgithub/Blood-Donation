// Import các namespace cần thiết cho ứng dụng
using Blood_Donation_Website.Data;
using Blood_Donation_Website.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Blood_Donation_Website.Services.Implementations;
using Blood_Donation_Website.Services.Interfaces;

// Khởi tạo WebApplication Builder
var builder = WebApplication.CreateBuilder(args);

// Cấu hình Controllers và Views với hỗ trợ biên dịch Razor runtime
// AddRazorRuntimeCompilation cho phép chỉnh sửa view mà không cần build lại
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Cấu hình Session - lưu trữ dữ liệu tạm thời của người dùng
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout sau 30 phút không hoạt động
    options.Cookie.HttpOnly = true; // Chỉ cho phép truy cập cookie qua HTTP (không qua JavaScript)
    options.Cookie.IsEssential = true; // Cookie này cần thiết cho ứng dụng hoạt động
});

// Cấu hình Database Context với SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký AutoMapper - công cụ map tự động giữa các object (DTO, Entity, ViewModel)
builder.Services.AddAutoMapper(typeof(Program));

// === ĐĂNG KÝ CÁC SERVICES - Dependency Injection ===
// Đăng ký các service với lifetime Scoped (1 instance per HTTP request)

// Email Service - gửi email thông báo, xác nhận
builder.Services.AddScoped<IEmailService, EmailService>();

// Account Service - quản lý đăng nhập, đăng ký, đổi mật khẩu
builder.Services.AddScoped<IAccountService, AccountService>();

// User Service - quản lý thông tin người dùng
builder.Services.AddScoped<IUserService, UserService>();

// Profile Service - quản lý hồ sơ cá nhân người dùng
builder.Services.AddScoped<IProfileService, ProfileService>();

// Blood Donation Event Service - quản lý sự kiện hiến máu
builder.Services.AddScoped<IBloodDonationEventService, BloodDonationEventService>();

// Contact Message Service - quản lý tin nhắn liên hệ từ người dùng
builder.Services.AddScoped<IContactMessageService, ContactMessageService>();

// Data Exporter - xuất dữ liệu ra file JSON/CSV
builder.Services.AddScoped<Blood_Donation_Website.Utilities.DataExporter>();

// Donation Registration Service - quản lý đăng ký hiến máu
builder.Services.AddScoped<IDonationRegistrationService, DonationRegistrationService>();

// Blood Type Service - quản lý thông tin nhóm máu
builder.Services.AddScoped<IBloodTypeService, BloodTypeService>();

// Location Service - quản lý địa điểm tổ chức hiến máu
builder.Services.AddScoped<ILocationService, LocationService>();

// Role Service - quản lý vai trò người dùng (Admin, Doctor, Staff, User...)
builder.Services.AddScoped<IRoleService, RoleService>();

// Donation History Service - quản lý lịch sử hiến máu
builder.Services.AddScoped<IDonationHistoryService, DonationHistoryService>();

// Health Screening Service - quản lý sàng lọc sức khỏe trước khi hiến máu
builder.Services.AddScoped<IHealthScreeningService, HealthScreeningService>();

// News Service - quản lý tin tức, bài viết
builder.Services.AddScoped<INewsService, NewsService>();

// === CẤU HÌNH XÁC THỰC (AUTHENTICATION) ===
// Sử dụng Cookie Authentication - lưu thông tin đăng nhập qua cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Đường dẫn trang đăng nhập
        options.LoginPath = "/Account/Login";
        
        // Đường dẫn trang đăng xuất
        options.LogoutPath = "/Account/Logout";
        
        // Đường dẫn trang thông báo không có quyền truy cập
        options.AccessDeniedPath = "/Account/AccessDenied";
        
        // Thời gian cookie tồn tại: 8 giờ
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        
        // Sliding expiration: gia hạn thời gian cookie khi người dùng hoạt động
        options.SlidingExpiration = true;
        
        // Chỉ cho phép truy cập cookie qua HTTP (không qua JavaScript - bảo mật)
        options.Cookie.HttpOnly = true;
        
        // Cookie bảo mật theo môi trường (HTTPS nếu có)
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        
        // Chế độ SameSite ngăn CSRF attacks
        options.Cookie.SameSite = SameSiteMode.Lax;
        
        // Tên cookie
        options.Cookie.Name = "BloodDonationAuth";
        
        // Sự kiện khi đăng xuất
        options.Events.OnSigningOut = async context =>
        {
            // Có thể thêm logic dọn dẹp thêm ở đây
            // Sự kiện này được kích hoạt khi gọi SignOutAsync
            await Task.CompletedTask;
        };
        
        // Sự kiện xác thực cookie
        options.Events.OnValidatePrincipal = async context =>
        {
            // Kiểm tra xem người dùng còn hợp lệ không
            // Có thể thêm logic kiểm tra bổ sung ở đây
            await Task.CompletedTask;
        };
    });

// Build ứng dụng với tất cả cấu hình đã đăng ký
var app = builder.Build();

// === SEED DỮ LIỆU MẪU (đã được comment out) ===
// Code này dùng để tạo database và seed dữ liệu mẫu lần đầu
// Uncomment khi cần khởi tạo dữ liệu
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     
//     // Tạo database nếu chưa tồn tại
//     context.Database.EnsureCreated();
//     
//     // Seed dữ liệu mẫu vào database
//     context.SeedData();
// }

// === CẤU HÌNH HTTP REQUEST PIPELINE ===
// Pipeline xử lý các request HTTP theo thứ tự

// Môi trường Production: sử dụng trang lỗi chung và HSTS
if (!app.Environment.IsDevelopment())
{
    // Trang xử lý lỗi chung
    app.UseExceptionHandler("/Home/Error");
    
    // HSTS: HTTP Strict Transport Security - bắt buộc dùng HTTPS
    app.UseHsts();
}

// Chuyển hướng HTTP sang HTTPS
app.UseHttpsRedirection();

// Phục vụ các file tĩnh (CSS, JS, hình ảnh) từ thư mục wwwroot
app.UseStaticFiles();

// Cấu hình routing - định tuyến URL đến Controller/Action
app.UseRouting();

// Kích hoạt Session middleware
app.UseSession();

// Kích hoạt Authentication middleware - xác thực người dùng
app.UseAuthentication();

// Kích hoạt Authorization middleware - phân quyền truy cập
app.UseAuthorization();

// Cấu hình route mặc định: {controller=Home}/{action=Index}/{id?}
// VD: /Home/Index, /Events/Details/5
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Chạy ứng dụng
app.Run();
