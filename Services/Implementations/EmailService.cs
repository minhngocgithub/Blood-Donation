using Blood_Donation_Website.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Blood_Donation_Website.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var smtpUsername = _configuration["EmailSettings:Username"] ?? "";
                var smtpPassword = _configuration["EmailSettings:Password"] ?? "";
                var fromEmail = _configuration["EmailSettings:FromEmail"] ?? smtpUsername;
                var fromName = _configuration["EmailSettings:FromName"] ?? "BloodLife";

                if (string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
                {
                    return true;
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (isHtml)
                {
                    bodyBuilder.HtmlBody = body;
                }
                else
                {
                    bodyBuilder.TextBody = body;
                }
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SendWelcomeEmailAsync(string email, string fullName)
        {
            var subject = "Chào mừng bạn đến với BloodLife!";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 30px; text-align: center;'>
                        <h1 style='color: white; margin: 0;'>Chào mừng đến với BloodLife!</h1>
                    </div>
                    <div style='padding: 30px; background: #f8f9fa;'>
                        <h2 style='color: #333;'>Xin chào {fullName},</h2>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Cảm ơn bạn đã tham gia cộng đồng hiến máu nhân ái BloodLife! 
                            Chúng tôi rất vui mừng chào đón bạn.
                        </p>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Với BloodLife, bạn có thể:
                        </p>
                        <ul style='font-size: 16px; line-height: 1.8; color: #555;'>
                            <li>Đăng ký tham gia các sự kiện hiến máu</li>
                            <li>Theo dõi lịch sử hiến máu của mình</li>
                            <li>Nhận thông báo về các sự kiện mới</li>
                            <li>Kết nối với cộng đồng hiến máu</li>
                        </ul>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='https://localhost:7000' style='background: #dc3545; color: white; padding: 15px 30px; text-decoration: none; border-radius: 25px; font-weight: bold;'>
                                Khám phá ngay
                            </a>
                        </div>
                        <p style='font-size: 14px; color: #888; text-align: center;'>
                            Cảm ơn bạn đã chọn BloodLife để lan tỏa yêu thương!
                        </p>
                    </div>
                </div>";

            return await SendEmailAsync(email, subject, body, true);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email, string resetToken, string resetUrl)
        {
            var subject = "Đặt lại mật khẩu - BloodLife";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: #dc3545; padding: 30px; text-align: center;'>
                        <h1 style='color: white; margin: 0;'>Đặt lại mật khẩu</h1>
                    </div>
                    <div style='padding: 30px; background: #f8f9fa;'>
                        <h2 style='color: #333;'>Yêu cầu đặt lại mật khẩu</h2>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.
                        </p>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Nhấn vào nút bên dưới để đặt lại mật khẩu:
                        </p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{resetUrl}?token={resetToken}&email={email}' 
                               style='background: #dc3545; color: white; padding: 15px 30px; text-decoration: none; border-radius: 25px; font-weight: bold;'>
                                Đặt lại mật khẩu
                            </a>
                        </div>
                        <p style='font-size: 14px; color: #888;'>
                            Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.
                        </p>
                        <p style='font-size: 14px; color: #888;'>
                            Link này sẽ hết hạn sau 24 giờ.
                        </p>
                    </div>
                </div>";

            return await SendEmailAsync(email, subject, body, true);
        }

        public async Task<bool> SendEmailVerificationAsync(string email, string verificationToken, string verificationUrl)
        {
            var subject = "Xác thực email - BloodLife";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: #28a745; padding: 30px; text-align: center;'>
                        <h1 style='color: white; margin: 0;'>Xác thực email</h1>
                    </div>
                    <div style='padding: 30px; background: #f8f9fa;'>
                        <h2 style='color: #333;'>Xác thực tài khoản của bạn</h2>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Cảm ơn bạn đã đăng ký tài khoản BloodLife!
                        </p>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Để hoàn tất quá trình đăng ký, vui lòng nhấn vào nút bên dưới để xác thực email:
                        </p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{verificationUrl}?token={verificationToken}&email={email}' 
                               style='background: #28a745; color: white; padding: 15px 30px; text-decoration: none; border-radius: 25px; font-weight: bold;'>
                                Xác thực email
                            </a>
                        </div>
                        <p style='font-size: 14px; color: #888;'>
                            Nếu bạn không thể nhấn vào nút, hãy copy link sau vào trình duyệt:
                        </p>
                        <p style='font-size: 12px; color: #666; word-break: break-all;'>
                            {verificationUrl}?token={verificationToken}&email={email}
                        </p>
                    </div>
                </div>";

            return await SendEmailAsync(email, subject, body, true);
        }

        public async Task<bool> SendEventReminderEmailAsync(string email, string eventName, DateTime eventDate)
        {
            var subject = $"Nhắc nhở: Sự kiện {eventName} - BloodLife";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: #17a2b8; padding: 30px; text-align: center;'>
                        <h1 style='color: white; margin: 0;'>Nhắc nhở sự kiện</h1>
                    </div>
                    <div style='padding: 30px; background: #f8f9fa;'>
                        <h2 style='color: #333;'>Sự kiện sắp diễn ra!</h2>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Đây là lời nhắc nhở về sự kiện hiến máu mà bạn đã đăng ký:
                        </p>
                        <div style='background: white; padding: 20px; border-radius: 10px; margin: 20px 0;'>
                            <h3 style='color: #dc3545; margin-top: 0;'>{eventName}</h3>
                            <p style='font-size: 16px; color: #333;'>
                                <strong>Thời gian:</strong> {eventDate:dd/MM/yyyy HH:mm}
                            </p>
                        </div>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Hãy nhớ mang theo giấy tờ tùy thân và đến đúng giờ nhé!
                        </p>
                        <p style='font-size: 14px; color: #888;'>
                            Cảm ơn bạn đã tham gia hoạt động hiến máu nhân ái!
                        </p>
                    </div>
                </div>";

            return await SendEmailAsync(email, subject, body, true);
        }

        public async Task<bool> SendDonationConfirmationEmailAsync(string email, string eventName, DateTime donationDate)
        {
            var subject = "Cảm ơn bạn đã hiến máu - BloodLife";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: #28a745; padding: 30px; text-align: center;'>
                        <h1 style='color: white; margin: 0;'>Cảm ơn bạn!</h1>
                    </div>
                    <div style='padding: 30px; background: #f8f9fa;'>
                        <h2 style='color: #333;'>Hiến máu thành công!</h2>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Cảm ơn bạn đã tham gia hiến máu tại sự kiện:
                        </p>
                        <div style='background: white; padding: 20px; border-radius: 10px; margin: 20px 0;'>
                            <h3 style='color: #dc3545; margin-top: 0;'>{eventName}</h3>
                            <p style='font-size: 16px; color: #333;'>
                                <strong>Ngày hiến máu:</strong> {donationDate:dd/MM/yyyy}
                            </p>
                        </div>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Hành động của bạn có thể cứu sống đến 3 người. Đây thực sự là một việc làm ý nghĩa!
                        </p>
                        <p style='font-size: 16px; line-height: 1.6; color: #555;'>
                            Hãy chăm sóc sức khỏe và nghỉ ngơi đầy đủ. Bạn có thể hiến máu lại sau 3 tháng.
                        </p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='https://localhost:7000/profile/donation-history' style='background: #dc3545; color: white; padding: 15px 30px; text-decoration: none; border-radius: 25px; font-weight: bold;'>
                                Xem lịch sử hiến máu
                            </a>
                        </div>
                        <p style='font-size: 14px; color: #888; text-align: center;'>
                            Một lần nữa, cảm ơn bạn đã lan tỏa yêu thương!
                        </p>
                    </div>
                </div>";

            return await SendEmailAsync(email, subject, body, true);
        }
    }
}
