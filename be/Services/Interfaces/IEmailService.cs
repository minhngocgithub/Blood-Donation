namespace BloodDonationAPI.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task<bool> SendWelcomeEmailAsync(string email, string fullName);
        Task<bool> SendPasswordResetEmailAsync(string email, string resetToken, string resetUrl);
        Task<bool> SendEmailVerificationAsync(string email, string verificationToken, string verificationUrl);
        Task<bool> SendEventReminderEmailAsync(string email, string eventName, DateTime eventDate);
        Task<bool> SendDonationConfirmationEmailAsync(string email, string eventName, DateTime donationDate);
    }
}
