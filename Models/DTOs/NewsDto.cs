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
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public bool IsPublished { get; set; } = false;
    }

    public class NewsUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
    }
} 