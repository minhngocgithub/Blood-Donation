using System.Security.Cryptography;
using System.Text;

namespace Blood_Donation_Website.Services.Utilities
{
    /// <summary>
    /// Helper class xử lý mã hóa và xác thực mật khẩu
    /// Sử dụng thuật toán SHA256 với Salt để bảo mật
    /// </summary>
    public class PasswordHelper
    {
        /// <summary>
        /// Mã hóa mật khẩu với Salt ngẫu nhiên
        /// </summary>
        /// <param name="password">Mật khẩu thô (plain text)</param>
        /// <returns>Chuỗi mã hóa dạng "salt:hash"</returns>
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Tạo salt ngẫu nhiên (32 bytes)
                var salt = GenerateSalt();
                
                // Kết hợp password với salt
                var saltedPassword = password + salt;

                // Tính hash SHA256
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                var hashedPassword = Convert.ToBase64String(hashedBytes);

                // Trả về dạng "salt:hash" để lưu vào database
                return $"{salt}:{hashedPassword}";
            }
        }

        /// <summary>
        /// Xác thực mật khẩu - so sánh mật khẩu nhập vào với mật khẩu đã mã hóa
        /// </summary>
        /// <param name="password">Mật khẩu cần kiểm tra (plain text)</param>
        /// <param name="hashedPassword">Mật khẩu đã mã hóa từ database (dạng "salt:hash")</param>
        /// <returns>True nếu mật khẩu khớp, False nếu không khớp</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                // Tách salt và hash từ chuỗi đã lưu
                var parts = hashedPassword.Split(':');
                if (parts.Length != 2) return false; // Format không hợp lệ

                var salt = parts[0];
                var hash = parts[1];

                using (var sha256 = SHA256.Create())
                {
                    // Mã hóa mật khẩu nhập vào với cùng salt
                    var saltedPassword = password + salt;
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                    var computedHash = Convert.ToBase64String(hashedBytes);

                    // So sánh hash tính được với hash đã lưu
                    return hash == computedHash;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tạo chuỗi salt ngẫu nhiên 32 bytes
        /// </summary>
        /// <returns>Chuỗi salt dạng Base64</returns>
        private static string GenerateSalt()
        {
            var saltBytes = new byte[32]; // 32 bytes = 256 bits
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes); // Điền ngẫu nhiên an toàn
            }
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Tạo mật khẩu ngẫu nhiên - dùng cho chức năng quên mật khẩu
        /// </summary>
        /// <param name="length">Chiều dài mật khẩu (mặc định 12 ký tự)</param>
        /// <returns>Mật khẩu ngẫu nhiên chứa chữ hoa, chữ thường, số và ký tự đặc biệt</returns>
        public static string GenerateRandomPassword(int length = 12)
        {
            // Bộ ký tự cho phép trong mật khẩu
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            
            // Chọn ngẫu nhiên từ bộ ký tự
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
