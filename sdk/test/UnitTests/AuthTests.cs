using NUnit.Framework;
using Omnigage.Auth;

namespace UnitTests
{
    public class Tests
    {
        [Test]
        public void TestProps()
        {
            AuthContext auth = new AuthContext();

            auth.AccountKey = "sample";
            auth.TokenKey = "key";
            auth.TokenSecret = "secret";
            auth.Host = "https://omnigage.com";

            Assert.AreEqual(auth.AccountKey, "sample");
            Assert.AreEqual(auth.TokenKey, "key");
            Assert.AreEqual(auth.TokenSecret, "secret");
            Assert.AreEqual(auth.Host, "https://omnigage.com");
        }

        [Test]
        public void TestAuthorization()
        {
            AuthContext auth = new AuthContext();

            auth.TokenKey = "key";
            auth.TokenSecret = "secret";

            Assert.AreEqual(auth.Authorization, "a2V5OnNlY3JldA==");
        }
    }
}