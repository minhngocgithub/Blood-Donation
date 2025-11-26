using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.DTOs
{
    public class NewsDto
    {
        public int NewsId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public int ViewCount { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        // Navigation properties
        public string? CategoryName { get; set; }
        public string? AuthorName { get; set; }
    }

    public class NewsCreateDto
    {
        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được quá 200 ký tự")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Nội dung là bắt buộc")]
        public string Content { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Tóm tắt không được quá 500 ký tự")]
        public string? Summary { get; set; }
        
        [StringLength(255, ErrorMessage = "URL hình ảnh không được quá 255 ký tự")]
        public string? ImageUrl { get; set; }
        
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public bool IsPublished { get; set; } = false;
    }

    public class NewsUpdateDto
    {
        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được quá 200 ký tự")]
        public string Title { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Nội dung là bắt buộc")]
        public string Content { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Tóm tắt không được quá 500 ký tự")]
        public string? Summary { get; set; }
        
        [StringLength(255, ErrorMessage = "URL hình ảnh không được quá 255 ký tự")]
        public string? ImageUrl { get; set; }
        
        public int? CategoryId { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
} 