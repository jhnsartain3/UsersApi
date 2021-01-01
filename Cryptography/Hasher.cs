using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    public interface IHasher
    {
        string GenerateHash(string valueToHash);
    }

    public class Hasher : IHasher
    {
        public string GenerateHash(string valueToHash)
        {
            return GenerateSha512String(valueToHash);
        }

        private static string GenerateSha512String(string inputString)
        {
            var sha512 = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(inputString);
            var hash = sha512.ComputeHash(bytes);

            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(IEnumerable<byte> hash)
        {
            var result = new StringBuilder();

            foreach (var character in hash)
                result.Append(character.ToString("X2"));

            return result.ToString();
        }
    }
}