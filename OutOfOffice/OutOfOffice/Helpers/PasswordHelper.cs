using System.Security.Cryptography;
using System.Text;

namespace OutOfOffice.Helpers
{
    public static class PasswordHelper
    {
        private const int _saltSize = 32;

        public static string HashPassword (string password, string salt)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedPassword = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
            return Convert.ToHexString(hashedPassword);
        }

        public static string GenerateSalt()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(_saltSize));
        }

    }
}
