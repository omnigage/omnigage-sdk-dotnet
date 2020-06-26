using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Omnigage.Resources;
using RichardSzalay.MockHttp;

namespace UnitTests
{
    public class ResourceTests
    {
        [Test]
        async public Task TestVoiceEngagement()
        {
            var mockHttp = new MockHttpMessageHandler();
            var engagementResponse = File.ReadAllText("Serialized/ResourceTestsEngagementResponse.json");
            var activityResponse = File.ReadAllText("Serialized/ResourceTestsActivityResponse.json");
            var triggerResponse = File.ReadAllText("Serialized/ResourceTestsTriggerResponse.json");
            var envelopeRequest = File.ReadAllText("Serialized/ResourceTestsEnvelopeBulkRequest.json");

            mockHttp.When(HttpMethod.Post, "http://localhost/api/engagements")
                .Respond(System.Net.HttpStatusCode.Created, "application/json", engagementResponse);
            mockHttp.When(HttpMethod.Post, "http://localhost/api/activities")
                .Respond(System.Net.HttpStatusCode.Created, "application/json", activityResponse);
            mockHttp.When(HttpMethod.Post, "http://localhost/api/triggers")
                .Respond(System.Net.HttpStatusCode.Created, "application/json", triggerResponse);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/api/");

            EngagementModel engagement = new EngagementModel();
            engagement.Name = "Example";
            engagement.Direction = "outbound";
            engagement.Status = "scheduled";

            await engagement.Create(client);

            Assert.AreEqual(engagement.Id, "1");
            Assert.AreEqual(engagement.ToString(), engagementResponse);

            ActivityModel activity = new ActivityModel();
            activity.Name = "Voice Blast";
            activity.Kind = "voice";
            activity.Engagement = engagement;
            activity.CallerId = new CallerIdModel
            {
                Id = "yL9vQaWrSqg5W8EFEpE6xZ"
            };

            await activity.Create(client);

            Assert.AreEqual(activity.Id, "1");
            Assert.AreEqual(activity.Name, "Voice Blast");
            Assert.AreEqual(activity.Kind, "voice");
            Assert.AreEqual(activity.ToString(), activityResponse);

            TriggerModel trigger = new TriggerModel();
            trigger.Kind = "play";
            trigger.OnEvent = "voice-machine";
            trigger.Activity = activity;
            trigger.VoiceTemplate = new VoiceTemplateModel
            {
                Id = "RaF56o2r58hTKT7AYS9doj"
            };

            await trigger.Create(client);

            Assert.AreEqual(trigger.Id, "1");
            Assert.AreEqual(trigger.Kind, "play");
            Assert.AreEqual(trigger.OnEvent, "voice-machine");
            Assert.AreEqual(trigger.ToString(), triggerResponse);

            EnvelopeModel envelope = new EnvelopeModel();
            envelope.PhoneNumber = "+11111111111";
            envelope.Engagement = engagement;
            envelope.Meta = new Dictionary<string, string>
            {
                { "first-name", "Michael" },
                { "last-name", "Morgan" }
            };

            List<EnvelopeModel> envelopes = new List<EnvelopeModel> { };
            envelopes.Add(envelope);

            string envelopesPayload = EnvelopeModel.SerializeBulk(envelopes);
            Assert.AreEqual(envelopesPayload, envelopeRequest);
        }

        [Test]
        public void TestVoiceTemplateModel()
        {
            VoiceTemplateModel instance = new VoiceTemplateModel();

            instance.Name = "Example";
            instance.Kind = "audio";

            Assert.AreEqual(instance.Name, "Example");
            Assert.AreEqual(instance.Kind, "audio");
        }

        [Test]
        public void TestEngagementModel()
        {
            EngagementModel instance = new EngagementModel();

            instance.Name = "Example";
            instance.Direction = "outbound";
            instance.Status = "scheduled";

            Assert.AreEqual(instance.Name, "Example");
            Assert.AreEqual(instance.Direction, "outbound");
            Assert.AreEqual(instance.Status, "scheduled");
        }
    }
}