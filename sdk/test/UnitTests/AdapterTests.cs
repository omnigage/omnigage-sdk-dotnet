﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Omnigage;
using Omnigage.Runtime;
using RichardSzalay.MockHttp;

namespace Tests.UnitTests
{
    public class ArticleResource : Adapter
    {
        public override string Type { get; } = "articles";

        public string Title;

        public string Body;

        [JsonProperty(propertyName: "is-published")]
        public bool IsPublished;

        [JsonIgnore]
        public int Viewers { get; set; }
    }

    [TestFixture]
    public class UtilTests
    {
        public static ArticleResource fixture;

        [SetUp]
        public void FixtureSetUp()
        {
            ArticleResource article = new ArticleResource();
            article.Title = "Omnigage .NET SDK";
            article.Body = "The Omnigage API .NET bindings...";
            article.IsPublished = true;
            article.Viewers = 1;
            fixture = article;
        }

        [Test]
        public void TestProps()
        {
            Assert.That(fixture.Title, Does.EndWith("SDK"));
            Assert.That(fixture.Body, Does.EndWith("bindings..."));
            Assert.IsTrue(fixture.IsPublished);
            Assert.AreEqual(fixture.Viewers, 1);
        }

        [Test]
        public void TestToString()
        {
            string body = File.ReadAllText("UnitTests/Serialized/UtilTestsToString.json");
            Assert.AreEqual(fixture.ToString(), body);
        }

        [Test]
        public void TestSerialize()
        {
            string body = File.ReadAllText("UnitTests/Serialized/UtilTestsToString.json");
            Assert.AreEqual(fixture.Serialize(), body);
        }

        [Test]
        async public Task TestCreate()
        {
            var mockHttp = new MockHttpMessageHandler();
            var response = File.ReadAllText("UnitTests/Serialized/UtilTestsCreate.json");

            mockHttp.When(HttpMethod.Post, "http://localhost/api/articles")
                .Respond(System.Net.HttpStatusCode.Created, "application/json", response);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/api/");

            OmnigageClient.Init("key", "secret", "http://localhost/api/", client);

            await fixture.Create();

            Assert.AreEqual(fixture.Id, "1");
        }

#if NETCOREAPP3_1
        [Test]
        async public Task TestUpdate()
        {
            var mockHttp = new MockHttpMessageHandler();
            var response = File.ReadAllText("UnitTests/Serialized/UtilTestsCreate.json");

            mockHttp.When(HttpMethod.Post, "http://localhost/api/articles")
                .Respond(System.Net.HttpStatusCode.Created, "application/json", response);

            mockHttp.When(HttpMethod.Patch, "http://localhost/api/articles/*")
                .Respond(System.Net.HttpStatusCode.Accepted, "application/json", response);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/api/");

            OmnigageClient.Init("key", "secret", "http://localhost/api/", client);

            await fixture.Create();

            Assert.AreEqual(fixture.Id, "1");

            await fixture.Update();
        }
#endif

    }
}