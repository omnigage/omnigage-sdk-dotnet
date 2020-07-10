using System.Threading.Tasks;
using NUnit.Framework;
using Omnigage;
using Omnigage.Resources;
using Scotch;

namespace Tests.IntegrationTests
{
    public class EmailTests
    {
        [Test]
        async public Task TestEmailMessage()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/EmailMessage.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            Client.Init(tokenKey, tokenSecret, host, client);

            EmailMessageResource emailMessage = new EmailMessageResource();
            emailMessage.Subject = "Ahoy";
            emailMessage.Body = "Sample body";

            await emailMessage.Create();

            EmailResource email = new EmailResource();
            email.To = "demo@omnigage.com";
            email.EmailMessage = emailMessage;
            email.EmailId = new EmailIdResource
            {
                Id = "NbXW9TCHax9zfAeDhaY2bG"
            };

            await email.Create();
        }

        [Test]
        async public Task TestEmailTemplate()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/EmailTemplate.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            Client.Init(tokenKey, tokenSecret, host, client);

            EmailTemplateResource emailTemplate = new EmailTemplateResource();
            emailTemplate.Subject = "Ahoy";
            emailTemplate.Body = "Sample body";

            await emailTemplate.Create();

            EmailMessageResource emailMessage = new EmailMessageResource();
            emailMessage.EmailTemplate = emailTemplate;

            await emailMessage.Create();

            // The message instance should derive the subject/body from the template
            Assert.AreEqual(emailMessage.Subject, emailTemplate.Subject);
            Assert.AreEqual(emailMessage.Body, emailTemplate.Body);

            EmailResource email = new EmailResource();
            email.To = "demo@omnigage.com";
            email.EmailMessage = emailMessage;
            email.EmailId = new EmailIdResource
            {
                Id = "NbXW9TCHax9zfAeDhaY2bG"
            };

            await email.Create();
        }
    }
}
