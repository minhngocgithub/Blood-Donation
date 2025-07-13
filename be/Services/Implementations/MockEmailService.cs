using BloodDonationAPI.Services.Interfaces;

namespace BloodDonationAPI.Services.Implementations
{
    public class MockEmailService : IEmailService
    {
        private readonly ILogger<MockEmailService> _logger;

        public MockEmailService(ILogger<MockEmailService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            await Task.Delay(100); // Simulate async operation

            _logger.LogInformation("📧 [MOCK EMAIL] Sending email to: {To}", to);
            _logger.LogInformation("📧 [MOCK EMAIL] Subject: {Subject}", subject);
            _logger.LogInformation("📧 [MOCK EMAIL] Body type: {BodyType}", isHtml ? "HTML" : "Text");
            _logger.LogInformation("📧 [MOCK EMAIL] Body preview: {BodyPreview}", 
                body.Length > 100 ? body.Substring(0, 100) + "..." : body);
            
            // Simulate successful sending
            return true;
        }

        public async Task<bool> SendWelcomeEmailAsync(string email, string fullName)
        {
            _logger.LogInformation("🎉 [MOCK EMAIL] Sending welcome email to {Email} for {FullName}", email, fullName);
            
            var subject = "Chào mừng bạn đến với BloodLife!";
            var body = $"Xin chào {fullName}, chào mừng bạn đến với BloodLife!";
            
            return await SendEmailAsync(email, subject, body, true);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email, string resetToken, string resetUrl)
        {
            _logger.LogInformation("🔑 [MOCK EMAIL] Sending password reset email to {Email}", email);
            _logger.LogInformation("🔑 [MOCK EMAIL] Reset token: {ResetToken}", resetToken);
            
            var subject = "Đặt lại mật khẩu - BloodLife";
            var body = $"Reset your password using token: {resetToken}";
            
            return await SendEmailAsync(email, subject, body, true);
        }

        public async Task<bool> SendEmailVerificationAsync(string email, string verificationToken, string verificationUrl)
        {
            _logger.LogInformation("✅ [MOCK EMAIL] Sending email verification to {Email}", email);
            _logger.LogInformation("✅ [MOCK EMAIL] Verification token: {VerificationToken}", verificationToken);
            
            var subject = "Xác thực email - BloodLife";
            var body = $"Verify your email using token: {verificationToken}";
            
            return await SendEmailAsync(email, subject, body, true);
        }

        public async Task<bool> SendEventReminderEmailAsync(string email, string eventName, DateTime eventDate)
        {
            _logger.LogInformation("📅 [MOCK EMAIL] Sending event reminder to {Email} for event {EventName} on {EventDate}", 
                email, eventName, eventDate);
            
            var subject = $"Nhắc nhở: Sự kiện {eventName} - BloodLife";
            var body = $"Reminder: {eventName} on {eventDate:dd/MM/yyyy HH:mm}";
            
            return await SendEmailAsync(email, subject, body, true);
        }

        public async Task<bool> SendDonationConfirmationEmailAsync(string email, string eventName, DateTime donationDate)
        {
            _logger.LogInformation("💝 [MOCK EMAIL] Sending donation confirmation to {Email} for event {EventName} on {DonationDate}", 
                email, eventName, donationDate);
            
            var subject = "Cảm ơn bạn đã hiến máu - BloodLife";
            var body = $"Thank you for donating blood at {eventName} on {donationDate:dd/MM/yyyy}";
            
            return await SendEmailAsync(email, subject, body, true);
        }
    }
}
