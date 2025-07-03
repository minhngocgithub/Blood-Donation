using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("NewsCategory")]
    public class NewsCategory : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }

        public virtual ICollection<News> NewsArticles { get; set; } = new List<News>();
    }
}
