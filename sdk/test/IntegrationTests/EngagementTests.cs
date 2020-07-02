using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Omnigage;
using Omnigage.Resources;
using Omnigage.Util;
using Scotch;

namespace Tests.IntegrationTests
{
    public class EngagementTests
    {
        [Test]
        async public Task TestVoiceEngagement()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/EngagementTests.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string accountKey = "sandbox";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            Client.Init(tokenKey, tokenSecret, accountKey, host, client);

            EngagementResource engagement = new EngagementResource();
            engagement.Name = "Example Voice Blast";
            engagement.Direction = "outbound";

            await engagement.Create();

            ActivityResource activity = new ActivityResource();
            activity.Name = "Voice Blast";
            activity.Kind = "voice";
            activity.Engagement = engagement;
            activity.CallerId = new CallerIdResource
            {
                Id = "yL9vQaWrSqg5W8EFEpE6xZ"
            };

            await activity.Create();

            UploadResource upload1 = new UploadResource
            {
                FilePath = "Media/hello.mp3"
            };
            await upload1.Create();

            UploadResource upload2 = new UploadResource
            {
                FilePath = "Media/you-have-new-mail-waiting.wav"
            };
            await upload2.Create();

            VoiceTemplateResource humanRecording = new VoiceTemplateResource();
            humanRecording.Name = "Human Recording";
            humanRecording.Kind = "audio";
            humanRecording.Upload = upload1;
            await humanRecording.Create();

            VoiceTemplateResource machineRecording = new VoiceTemplateResource();
            machineRecording.Name = "Machine Recording";
            machineRecording.Kind = "audio";
            machineRecording.Upload = upload2;
            await machineRecording.Create();

            // Define human trigger
            TriggerResource triggerHumanInstance = new TriggerResource();
            triggerHumanInstance.Kind = "play";
            triggerHumanInstance.OnEvent = "voice-human";
            triggerHumanInstance.Activity = activity;
            triggerHumanInstance.VoiceTemplate = humanRecording;
            await triggerHumanInstance.Create();

            // Define machine trigger
            TriggerResource triggerMachineInstance = new TriggerResource();
            triggerMachineInstance.Kind = "play";
            triggerMachineInstance.OnEvent = "voice-machine";
            triggerMachineInstance.Activity = activity;
            triggerMachineInstance.VoiceTemplate = machineRecording;
            await triggerMachineInstance.Create();

            EnvelopeResource envelope = new EnvelopeResource();
            envelope.PhoneNumber = "+18332676094";
            envelope.Engagement = engagement;
            envelope.Meta = new Dictionary<string, string>
            {
                { "first-name", "Spectrum" },
                { "last-name", "Support" }
            };

            // Push one or more envelopes into list
            List<EnvelopeResource> envelopes = new List<EnvelopeResource> { };
            envelopes.Add(envelope);

            // Populate engagement queue
            await Adapter.PostBulkRequest("envelopes", EnvelopeResource.SerializeBulk(envelopes));

            // Schedule engagement for processing
            engagement.Status = "scheduled";
            await engagement.Update();
        }
    }
}
