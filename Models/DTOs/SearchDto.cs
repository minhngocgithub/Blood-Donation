namespace Blood_Donation_Website.Models.DTOs
{
    public class SearchParametersDto
    {
        public string? SearchTerm { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc";
    }

    public class EventSearchDto : SearchParametersDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? LocationId { get; set; }
        public string? Status { get; set; }
        public string? RequiredBloodTypes { get; set; }
    }

    public class UserSearchDto : SearchParametersDto
    {
        public int? BloodTypeId { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public bool? EmailVerified { get; set; }
        public string? Gender { get; set; }
    }

    public class DonationSearchDto : SearchParametersDto
    {
        public int? UserId { get; set; }
        public int? EventId { get; set; }
        public int? BloodTypeId { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? CertificateIssued { get; set; }
    }

    public class NewsSearchDto : SearchParametersDto
    {
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public bool? IsPublished { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
} 