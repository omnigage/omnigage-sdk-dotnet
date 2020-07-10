using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Omnigage.Core;
using Omnigage.Resource;
using RichardSzalay.MockHttp;

namespace Tests.UnitTests
{
    public class ResourceTests
    {
        [Test]
        async public Task TestVoiceEngagement()
        {
            var mockHttp = new MockHttpMessageHandler();
            var engagementResponse = File.ReadAllText("UnitTests/Serialized/ResourceTestsEngagementResponse.json");
            var activityResponse = File.ReadAllText("UnitTests/Serialized/ResourceTestsActivityResponse.json");
            var triggerResponse = File.ReadAllText("UnitTests/Serialized/ResourceTestsTriggerResponse.json");
            var envelopeRequest = File.ReadAllText("UnitTests/Serialized/ResourceTestsEnvelopeBulkRequest.json");

            mockHttp.When(HttpMethod.Post, "http://localhost/api/engagements")
                .Respond(System.Net.HttpStatusCode.Created, "application/json", engagementResponse);
            mockHttp.When(HttpMethod.Post, "http://localhost/api/activities")
                .Respond(System.Net.HttpStatusCode.Created, "application/json", activityResponse);
            mockHttp.When(HttpMethod.Post, "http://localhost/api/triggers")
                .Respond(System.Net.HttpStatusCode.Created, "application/json", triggerResponse);

            var httpClient = mockHttp.ToHttpClient();

            Client.Init("key", "secret", "http://localhost/api/", httpClient);

            EngagementResource engagement = new EngagementResource();
            engagement.Name = "Example";
            engagement.Direction = "outbound";
            engagement.Status = "scheduled";

            await engagement.Create();

            Assert.AreEqual(engagement.Id, "1");
            Assert.AreEqual(engagement.ToString(), engagementResponse);

            ActivityResource activity = new ActivityResource();
            activity.Name = "Voice Blast";
            activity.Kind = "voice";
            activity.Engagement = engagement;
            activity.CallerId = new CallerIdResource
            {
                Id = "yL9vQaWrSqg5W8EFEpE6xZ"
            };

            await activity.Create();

            Assert.AreEqual(activity.Id, "1");
            Assert.AreEqual(activity.Name, "Voice Blast");
            Assert.AreEqual(activity.Kind, "voice");

            TriggerResource trigger = new TriggerResource();
            trigger.Kind = "play";
            trigger.OnEvent = "voice-machine";
            trigger.Activity = activity;
            trigger.VoiceTemplate = new VoiceTemplateResource
            {
                Id = "RaF56o2r58hTKT7AYS9doj"
            };

            await trigger.Create();

            Assert.AreEqual(trigger.Id, "1");
            Assert.AreEqual(trigger.Kind, "play");
            Assert.AreEqual(trigger.OnEvent, "voice-machine");

            EnvelopeResource envelope = new EnvelopeResource();
            envelope.PhoneNumber = "+11111111111";
            envelope.Engagement = engagement;
            envelope.Meta = new Dictionary<string, string>
            {
                { "first-name", "Michael" },
                { "last-name", "Morgan" }
            };

            List<EnvelopeResource> envelopes = new List<EnvelopeResource> { };
            envelopes.Add(envelope);

            string envelopesPayload = EnvelopeResource.SerializeBulk(envelopes);
            Assert.AreEqual(envelopesPayload, envelopeRequest);
        }

        [Test]
        public void TestVoiceTemplateResource()
        {
            VoiceTemplateResource instance = new VoiceTemplateResource();

            instance.Name = "Example";
            instance.Kind = "audio";

            Assert.AreEqual(instance.Name, "Example");
            Assert.AreEqual(instance.Kind, "audio");
        }

        [Test]
        public void TestEngagementResource()
        {
            EngagementResource instance = new EngagementResource();

            instance.Name = "Example";
            instance.Direction = "outbound";
            instance.Status = "scheduled";

            Assert.AreEqual(instance.Name, "Example");
            Assert.AreEqual(instance.Direction, "outbound");
            Assert.AreEqual(instance.Status, "scheduled");
        }
    }
}