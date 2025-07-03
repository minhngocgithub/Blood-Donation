using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("News")]
    public class News : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Summary { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public int ViewCount { get; set; } = 0;
        public bool IsPublished { get; set; } = false;
        public DateTime? PublishedDate { get; set; }

   
        public virtual NewsCategory? Category { get; set; }
        public virtual User? Author { get; set; }
    }
}