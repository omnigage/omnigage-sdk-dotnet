using System.Collections.Generic;
using Newtonsoft.Json;
using Omnigage.Runtime;

namespace Omnigage.Resource
{
    public class ActivityKind
    {
        public const string Voice = "voice";
        public const string Text = "text";
        public const string TextVoice = "text-voice";
        public const string Email = "email";
        public const string EmailVoice = "email-voice";
        public const string Dial = "dial";
    }

    /// <summary>
    /// Resource: `/activities` - https://omnigage.docs.apiary.io/#reference/engagement-resources/activity-collection
    /// </summary>
    public class ActivityResource : Adapter
    {
        public override string Type { get; } = "activities";

        public string Name;

        public string Kind;

        public string CallbackPhoneNumber;

        public EngagementResource Engagement;

        [JsonProperty(propertyName: "caller-id")]
        public CallerIdResource CallerId;

        [JsonProperty(propertyName: "email-id")]
        public EmailIdResource EmailId;

        [JsonProperty(propertyName: "phone-number")]
        public PhoneNumberResource PhoneNumber;

        [JsonProperty(propertyName: "email-template")]
        public EmailTemplateResource EmailTemplate;

        [JsonProperty(propertyName: "email-message")]
        public EmailMessageResource EmailMessage;

        [JsonProperty(propertyName: "text-template")]
        public TextTemplateResource TextTemplate;

        [JsonProperty(propertyName: "voice-template")]
        public VoiceTemplateResource VoiceTemplate;

        [JsonProperty(propertyName: "voice-templates")]
        public List<VoiceTemplateResource> VoiceTemplates { get; set; }
    }
}
