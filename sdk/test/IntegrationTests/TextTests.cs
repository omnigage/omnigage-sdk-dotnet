using System.Threading.Tasks;
using NUnit.Framework;
using Omnigage.Core;
using Omnigage.Resource;
using Scotch;

namespace Tests.IntegrationTests
{
    public class TextTests
    {
        [Test]
        async public Task TestTextMessage()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/TextMessage.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            Client.Init(tokenKey, tokenSecret, host, client);

            TextMessageResource textMessage = new TextMessageResource();
            textMessage.Body = "Sample body";

            await textMessage.Create();

            TextResource text = new TextResource();
            text.To = "+14076413749";
            text.TextMessage = textMessage;
            text.PhoneNumber = new PhoneNumberResource
            {
                Id = "GncieHvbCKfMYXmeycoWZm"
            };

            await text.Create();
        }

        [Test]
        async public Task TestTextTemplate()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/TextTemplate.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            Client.Init(tokenKey, tokenSecret, host, client);

            TextTemplateResource textTemplate = new TextTemplateResource();
            textTemplate.Name = "Text Template";
            textTemplate.Body = "Sample body";

            await textTemplate.Create();

            TextMessageResource textMessage = new TextMessageResource();
            textMessage.TextTemplate = textTemplate;

            await textMessage.Create();

            // The message instance should derive the body from the template
            Assert.AreEqual(textMessage.Body, textTemplate.Body);

            TextResource text = new TextResource();
            text.To = "+14076413749";
            text.TextMessage = textMessage;
            text.PhoneNumber = new PhoneNumberResource
            {
                Id = "GncieHvbCKfMYXmeycoWZm"
            };

            await text.Create();
        }
    }
}
