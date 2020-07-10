using NUnit.Framework;
using Omnigage.Runtime;

namespace Tests.UnitTests
{
    public class AuthTests
    {
        [Test]
        public void TestProps()
        {
            AuthContext auth = new AuthContext();

            auth.TokenKey = "key";
            auth.TokenSecret = "secret";

            Assert.AreEqual(auth.TokenKey, "key");
            Assert.AreEqual(auth.TokenSecret, "secret");
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