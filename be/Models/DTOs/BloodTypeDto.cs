namespace BloodDonationAPI.Models.DTOs
{
    public class BloodTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool CanGiveTo { get; set; }
        public bool CanReceiveFrom { get; set; }
    }
}
