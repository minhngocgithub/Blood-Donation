using Microsoft.AspNetCore.Mvc;

namespace BloodDonationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloodDonationController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetDonations()
        {
            var donations = new[]
            {
                new { Id = 1, DonorName = "Nguyễn Văn A", BloodType = "O+", Date = DateTime.Now.AddDays(-1) },
                new { Id = 2, DonorName = "Trần Thị B", BloodType = "A+", Date = DateTime.Now.AddDays(-2) },
                new { Id = 3, DonorName = "Lê Văn C", BloodType = "B+", Date = DateTime.Now.AddDays(-3) }
            };

            return Ok(donations);
        }

        [HttpPost]
        public IActionResult CreateDonation([FromBody] DonationRequest request)
        {
            // Xử lý logic tạo donation mới
            var newDonation = new
            {
                Id = new Random().Next(1000, 9999),
                DonorName = request.DonorName,
                BloodType = request.BloodType,
                Date = DateTime.Now
            };

            return Ok(newDonation);
        }

        [HttpGet("{id}")]
        public IActionResult GetDonation(int id)
        {
            var donation = new { Id = id, DonorName = "Sample Donor", BloodType = "O+", Date = DateTime.Now };
            return Ok(donation);
        }
    }

    public class DonationRequest
    {
        public string DonorName { get; set; } = "";
        public string BloodType { get; set; } = "";
    }
}
