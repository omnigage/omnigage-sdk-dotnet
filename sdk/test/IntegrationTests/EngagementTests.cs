﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Omnigage;
using Omnigage.Resource;
using Omnigage.Runtime;
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
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            OmnigageClient.Init(tokenKey, tokenSecret, host, client);

            EngagementResource engagement = new EngagementResource();
            engagement.Name = "Example Voice Blast";
            engagement.Direction = "outbound";

            await engagement.Create();

            ActivityResource activity = new ActivityResource();
            activity.Name = "Voice Blast";
            activity.Kind = ActivityKind.Voice;
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
            triggerHumanInstance.Kind = TriggerKind.Play;
            triggerHumanInstance.OnEvent = TriggerOnEvent.VoiceHuman;
            triggerHumanInstance.Activity = activity;
            triggerHumanInstance.VoiceTemplate = humanRecording;
            await triggerHumanInstance.Create();

            // Define machine trigger
            TriggerResource triggerMachineInstance = new TriggerResource();
            triggerMachineInstance.Kind = TriggerKind.Play;
            triggerMachineInstance.OnEvent = TriggerOnEvent.VoiceMachine;
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
            await OmnigageClient.PostBulkRequest("envelopes", EnvelopeResource.SerializeBulk(envelopes));

            // Schedule engagement for processing
            engagement.Status = "scheduled";
            await engagement.Update();
        }

        [Test]
        async public Task TestEmailEngagement()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/EngagementEmailTests.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            OmnigageClient.Init(tokenKey, tokenSecret, host, client);

            EngagementResource engagement = new EngagementResource();
            engagement.Name = "Example Email Blast";
            engagement.Direction = "outbound";

            await engagement.Create();

            EmailTemplateResource emailTemplate = new EmailTemplateResource();
            emailTemplate.Subject = "Ahoy";
            emailTemplate.Body = "Sample body";

            await emailTemplate.Create();

            ActivityResource activity = new ActivityResource();
            activity.Name = "Email Blast";
            activity.Kind = ActivityKind.Email;
            activity.Engagement = engagement;
            activity.EmailTemplate = emailTemplate;
            activity.EmailId = new EmailIdResource
            {
                Id = "NbXW9TCHax9zfAeDhaY2bG"
            };

            await activity.Create();

            EnvelopeResource envelope = new EnvelopeResource();
            envelope.EmailAddress = "demo@omnigage.com";
            envelope.Engagement = engagement;
            envelope.Meta = new Dictionary<string, string>
            {
                { "first-name", "Omnigage" },
                { "last-name", "Demo" }
            };

            // Push one or more envelopes into list
            List<EnvelopeResource> envelopes = new List<EnvelopeResource> { };
            envelopes.Add(envelope);

            // Populate engagement queue
            await OmnigageClient.PostBulkRequest("envelopes", EnvelopeResource.SerializeBulk(envelopes));

            // Schedule engagement for processing
            engagement.Status = "scheduled";
            await engagement.Update();
        }

        [Test]
        async public Task TestTextEngagement()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/EngagementTextTests.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            OmnigageClient.Init(tokenKey, tokenSecret, host, client);

            EngagementResource engagement = new EngagementResource();
            engagement.Name = "Example Text Blast";
            engagement.Direction = "outbound";

            await engagement.Create();

            TextTemplateResource textTemplate = new TextTemplateResource();
            textTemplate.Name = "Text Template";
            textTemplate.Body = "Sample body";

            await textTemplate.Create();

            ActivityResource activity = new ActivityResource();
            activity.Name = "Text Blast";
            activity.Kind = ActivityKind.Text;
            activity.Engagement = engagement;
            activity.TextTemplate = textTemplate;
            activity.PhoneNumber = new PhoneNumberResource
            {
                Id = "GncieHvbCKfMYXmeycoWZm"
            };

            await activity.Create();

            EnvelopeResource envelope = new EnvelopeResource();
            envelope.PhoneNumber = "+14076413749";
            envelope.Engagement = engagement;
            envelope.Meta = new Dictionary<string, string>
            {
                { "first-name", "Omnigage" },
                { "last-name", "Demo" }
            };

            // Push one or more envelopes into list
            List<EnvelopeResource> envelopes = new List<EnvelopeResource> { };
            envelopes.Add(envelope);

            // Populate engagement queue
            await Client.PostBulkRequest("envelopes", EnvelopeResource.SerializeBulk(envelopes));

            // Schedule engagement for processing
            engagement.Status = "scheduled";
            await engagement.Update();
        }

        [Test]
        async public Task TestEmailVoiceEngagement()
        {
            var scotchMode = ScotchMode.Replaying;
            var client = HttpClients.NewHttpClient("IntegrationTests/Cassettes/EngagementEmailVoiceTests.json", scotchMode);

            string tokenKey = "key";
            string tokenSecret = "secret";
            string host = "https://dvfoa3pu2rxx6.cloudfront.net/api/v1/";

            OmnigageClient.Init(tokenKey, tokenSecret, host, client);

            EngagementResource engagement = new EngagementResource();
            engagement.Name = "Example Email Voice Blast (Voice Link)";
            engagement.Direction = "outbound";
            await engagement.Create();

            EmailTemplateResource emailTemplate = new EmailTemplateResource();
            emailTemplate.Subject = "Email Template";
            emailTemplate.Body = "Email template with {{message-body}}.";
            await emailTemplate.Create();

            EmailMessageResource emailMessage = new EmailMessageResource();
            emailMessage.Subject = "Email Message";
            emailMessage.Body = "Sample body";
            emailMessage.IsDraft = true;
            await emailMessage.Create();

            UploadResource upload1 = new UploadResource
            {
                FilePath = "Media/hello.mp3"
            };
            await upload1.Create();

            VoiceTemplateResource recording = new VoiceTemplateResource();
            recording.Name = "Voice Link Recording";
            recording.Kind = "audio";
            recording.Upload = upload1;
            await recording.Create();

            ActivityResource activity = new ActivityResource();
            activity.Name = "Email Voice";
            activity.Kind = ActivityKind.EmailVoice;
            activity.CallBackPhoneNumber = "+11231231234";
            activity.Engagement = engagement;
            activity.VoiceTemplate = recording;
            activity.EmailTemplate = emailTemplate;
            activity.EmailMessage = emailMessage;
            activity.EmailId = new EmailIdResource
            {
                Id = "NbXW9TCHax9zfAeDhaY2bG"
            };
            await activity.Create();

            EnvelopeResource envelope = new EnvelopeResource();
            envelope.EmailAddress = "demo@omnigage.com";
            envelope.Engagement = engagement;
            envelope.Meta = new Dictionary<string, string>
            {
                { "first-name", "Omnigage" },
                { "last-name", "Demo" }
            };

            // Push one or more envelopes into list
            List<EnvelopeResource> envelopes = new List<EnvelopeResource> { };
            envelopes.Add(envelope);

            // Populate engagement queue
            await OmnigageClient.PostBulkRequest("envelopes", EnvelopeResource.SerializeBulk(envelopes));

            // Schedule engagement for processing
            engagement.Status = "scheduled";
            await engagement.Update();
        }
    }
}
