using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.Entities
{
    public class NewsCategory : BaseEntity
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        
        public virtual ICollection<News> NewsArticles { get; set; } = new List<News>();
    }
}
