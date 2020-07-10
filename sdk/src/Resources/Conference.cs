using Newtonsoft.Json;
using Omnigage.Runtime;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/conferences` - https://omnigage.docs.apiary.io/#reference/phone-resources/conference-collection
    /// </summary>
    public class ConferenceResource : Adapter
    {
        public override string Type { get; } = "conferences";

        public string Label { get; set; }

        [JsonProperty(propertyName: "beep-on-enter")]
        public bool BeepOnEnter { get; set; }

        [JsonProperty(propertyName: "beep-on-exit")]
        public bool BeepOnExit { get; set; }

        [JsonProperty(propertyName: "play-hold-music")]
        public bool PlayHoldMusic { get; set; }

        [JsonProperty(propertyName: "max-participants")]
        public string MaxParticipants { get; set; }

        [JsonProperty(propertyName: "is-muted")]
        public bool IsMuted { get; set; }
    }
}
