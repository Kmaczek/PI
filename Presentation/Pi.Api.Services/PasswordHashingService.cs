using Pi.Api.Services.Interfaces;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Pi.Api.Services
{
    public class PasswordHashingService : IPasswordHashing
    {
        public PasswordHashingService()
        {
        }

        public HashedPassword HashPassword(string password)
        {
            var result = new HashedPassword();
            result.Salt = GetSalt(50);
            result.Password = Hash(password, result.Salt);

            return result;
        }

        public bool ComparePasswords(string plainPassword, byte[] userSalt, byte[] hashedPassword)
        {
            var passwordHash = Hash(plainPassword, userSalt);
            var comparisonResult = Enumerable.SequenceEqual(passwordHash, hashedPassword);

            return comparisonResult;
        }

        public static byte[] Hash(string value, byte[] salt)
        {
            return Hash(Encoding.UTF8.GetBytes(value), salt);
        }

        private static byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();

            return new SHA256Managed().ComputeHash(saltedValue);
        }

        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }
    }

    public class HashedPassword
    {
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        
    }
}
