﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blood_Donation_Website.Models.Entities
{
    [Table("ContactMessages")]
    public class ContactMessage : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(15)]
        public string? Phone { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;

        [StringLength(20)]
        public string Status { get; set; } = "New";

        public DateTime? ResolvedDate { get; set; }
        public int? ResolvedBy { get; set; }

        [ForeignKey("ResolvedBy")]
        public virtual User? ResolvedByUser { get; set; }
    }
}
