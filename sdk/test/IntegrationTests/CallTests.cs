using System.Threading.Tasks;
using NUnit.Framework;
using Omnigage;
using Omnigage.Resource;
using Scotch;

namespace Tests.IntegrationTests
{
    public class CallTests
    {
        [Test]
        async public Task TestCallParent()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/CallParentTests.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            OmnigageClient.Init(tokenKey, tokenSecret, host, client);

            CallResource call = new CallResource();
            call.To = "+18332676094";
            call.Action = CallAction.Dial;

            await call.Create();
        }

        [Test]
        async public Task TestCallChild()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/CallChildTests.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            OmnigageClient.Init(tokenKey, tokenSecret, host, client);
            OmnigageClient.IsTesting = true;

            CallResource parentCall = new CallResource();
            parentCall.To = "+18332676094";
            parentCall.Action = CallAction.Dial;

            await parentCall.Create();

            while (true)
            {
                if (parentCall.Status == "in-progress")
                {
                    break;
                }
                else
                {
                    await parentCall.Reload();
                }
            }

            CallResource childCall = new CallResource();
            childCall.To = "+14076413749";
            childCall.Action = CallAction.Dial;
            childCall.ParentCall = parentCall;
            childCall.CallerId = new CallerIdResource
            {
                Id = "yL9vQaWrSqg5W8EFEpE6xZ"
            };

            await childCall.Create();
        }

        [Test]
        async public Task TestCallParentChild()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/CallParentChildTests.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            OmnigageClient.Init(tokenKey, tokenSecret, host, client);
            OmnigageClient.IsTesting = true;

            CallResource call = new CallResource();
            call.From = "+18332676094";
            call.To = "+14076413749";
            call.Action = CallAction.Dial;
            call.CallerId = new CallerIdResource
            {
                Id = "yL9vQaWrSqg5W8EFEpE6xZ"
            };

            await call.Create();
        }
    }
}
