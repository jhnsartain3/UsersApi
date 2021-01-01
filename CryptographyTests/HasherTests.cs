using Cryptography;
using NUnit.Framework;

namespace CryptographyTests
{
    public class HasherTests
    {
        private Hasher _hasher;

        [SetUp]
        public void Setup()
        {
            _hasher = new Hasher();
        }

        [Test]
        public void GenerateHash_CreatesStringOf100Characters()
        {
            Assert.AreEqual(128, _hasher.GenerateHash("fdsaf").Length);
        }
    }
}