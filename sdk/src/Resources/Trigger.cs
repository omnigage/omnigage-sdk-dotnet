using Newtonsoft.Json;
using Omnigage.Util;

namespace Omnigage.Resources
{
    /// <summary>
    /// Resource: `/triggers` - https://omnigage.docs.apiary.io/#reference/engagement-resources/trigger-collection
    /// </summary>
    public class TriggerResource : Adapter
    {
        public override string Type { get; } = "triggers";

        public string Kind;

        [JsonProperty(propertyName: "on-event")]
        public string OnEvent;

        [JsonProperty(propertyName: "voice-template")]
        public VoiceTemplateResource VoiceTemplate;

        public ActivityResource Activity;
    }
}
