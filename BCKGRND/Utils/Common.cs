using BCKGRND.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BCKGRND.Utils
{
    public class Common
    {
        public static byte[] GetRandomSalt(int length)
        {
            var random = RandomNumberGenerator.Create();
            byte[] salt = new byte[length];
            random.GetNonZeroBytes(salt);
            return salt;
        }

        public static byte[] SaltHashPassword(byte[] password, byte[] salt)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] plainTextWithSaltBytes = new byte[password.Length + salt.Length];
            for (int i = 0; i < password.Length; i++)
            {
                plainTextWithSaltBytes[i] = password[i];
            }
            for(int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[password.Length + i] = salt[i];
            }
            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }
    }
}
