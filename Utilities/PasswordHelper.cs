using System.Security.Cryptography;
using System.Text;


namespace Blood_Donation_Website.Services.Utilities
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var salt = GenerateSalt();
                var saltedPassword = password + salt;

                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                var hashedPassword = Convert.ToBase64String(hashedBytes);

                return $"{salt}:{hashedPassword}";
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                var parts = hashedPassword.Split(':');
                if (parts.Length != 2) return false;

                var salt = parts[0];
                var hash = parts[1];

                using (var sha256 = SHA256.Create())
                {
                    var saltedPassword = password + salt;
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                    var computedHash = Convert.ToBase64String(hashedBytes);

                    return hash == computedHash;
                }
            }
            catch
            {
                return false;
            }
        }

        private static string GenerateSalt()
        {
            var saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public static string GenerateRandomPassword(int length = 12)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
