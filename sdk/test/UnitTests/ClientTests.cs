using System.Net.Http;
using NUnit.Framework;
using Omnigage;
using Omnigage.Runtime;

namespace Tests.UnitTests
{
    public class ClientTests
    {
        [Test]
        public void TestDefaultProps()
        {
            OmnigageClient.Init("key", "secret");

            Assert.AreEqual(Client.Host, "https://api.omnigage.io/api/v1/");
            Assert.IsInstanceOf<HttpClient>(Client.HttpClient);
        }

        [Test]
        public void TestProps()
        {
            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "http://localhost/api/";

            OmnigageClient.Init(tokenKey, tokenSecret, host);

            Assert.AreEqual(Client.Host, host);
            Assert.AreEqual(Client.Auth.Authorization, "a2V5OnNlY3JldA==");
            Assert.IsInstanceOf<HttpClient>(Client.HttpClient);
        }
    }
}