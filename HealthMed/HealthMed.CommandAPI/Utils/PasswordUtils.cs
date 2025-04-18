using System.Security.Cryptography;

namespace HealthMed.CommandAPI.Utils
{
    public static class PasswordUtils
    {
        public static string GenerateSecurityHash()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string securityHash)
        {
            byte[] saltBytes = Convert.FromBase64String(securityHash);
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hashBytes = rfc2898DeriveBytes.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}