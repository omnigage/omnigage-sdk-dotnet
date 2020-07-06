using System.Collections.Generic;
using Newtonsoft.Json;
using Omnigage.Util;

namespace Omnigage.Resources
{
    /// <summary>
    /// Resource: `/calls` - https://omnigage.docs.apiary.io/#reference/call-resources/call-collection
    /// </summary>
    public class CallResource : Adapter
    {
        public const string DIAL = "dial";
        public const string HANGUP = "hangup";
        public const string ANSWER = "answer";
        public const string DECLINE = "decline";
        public const string HOLD = "hold";
        public const string RECORD = "record";
        public const string PLAY = "play";
        public const string PLAY_DROP = "play-drop";
        public const string VOICEMAIL_DROP = "voicemail-drop";
        public const string TRANSFER = "transfer";
        public const string CONFERENCE = "conference";

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
