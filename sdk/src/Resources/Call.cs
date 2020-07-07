using System.Collections.Generic;
using Newtonsoft.Json;
using Omnigage.Util;

namespace Omnigage.Resources
{
    public class CallAction
    {
        public const string Dial = "dial";
        public const string Hangup = "hangup";
        public const string Answer = "answer";
        public const string Decline = "decline";
        public const string Hold = "hold";
        public const string Record = "record";
        public const string Play = "play";
        public const string PlayDrop = "play-drop";
        public const string VoicemailDrop = "voicemail-drop";
        public const string Transfer = "transfer";
        public const string Conference = "conference";
    }

    /// <summary>
    /// Resource: `/calls` - https://omnigage.docs.apiary.io/#reference/call-resources/call-collection
    /// </summary>
    public class CallResource : Adapter
    {
        public override string Type { get; } = "calls";

        public string From { get; set; }

        public string To { get; set; }

        public string Action { get; set; }

        public string Status { get; set; }

        public string Direction { get; set; }

        public string Kind { get; set; }

        public string Callback { get; set; }

        [JsonProperty(propertyName: "meta_prop")]
        public Dictionary<string, string> Meta;

        [JsonProperty(propertyName: "started-at")]
        public string StartedAt { get; set; }

        [JsonProperty(propertyName: "finished-at")]
        public string FinishedAt { get; set; }

        [JsonProperty(propertyName: "created-at")]
        public string CreatedAt { get; set; }

        [JsonProperty(propertyName: "updated-at")]
        public string UpdatedAt { get; set; }

        [JsonProperty(propertyName: "caller-id")]
        public CallerIdResource CallerId;

        [JsonProperty(propertyName: "parent-call")]
        public CallResource ParentCall;

        public EnvelopeResource Envelope;

        [JsonProperty(propertyName: "voice-template")]
        public VoiceTemplateResource VoiceTemplate;

        public override string Serialize()
        {
            string serialized = base.Serialize();

            return serialized.Replace("meta_prop", "meta");
        }
    }
}
