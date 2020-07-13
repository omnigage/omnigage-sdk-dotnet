using System.Net;
using System.Net.Http;
using NUnit.Framework;
using Omnigage;
using Omnigage.Runtime;
using RichardSzalay.MockHttp;

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

        [Test]
        public void TestUnauthorizedException()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(HttpMethod.Post, "http://localhost/api/test")
                .Respond(HttpStatusCode.Unauthorized, "application/json", "invalid_client");

            var httpClient = mockHttp.ToHttpClient();

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "http://localhost/api/";

            OmnigageClient.Init(tokenKey, tokenSecret, host, httpClient);

            Assert.ThrowsAsync<AuthException>(() => Client.SendClientRequest("POST", "test"));
        }

        [Test]
        public void TestValidationException()
        {
            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When(HttpMethod.Post, "http://localhost/api/test")
                .Respond(HttpStatusCode.BadRequest, "application/json", "");

            var httpClient = mockHttp.ToHttpClient();

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "http://localhost/api/";

            OmnigageClient.Init(tokenKey, tokenSecret, host, httpClient);

            Assert.ThrowsAsync<ValidationException>(() => Client.SendClientRequest("POST", "test"));
        }
    }
}