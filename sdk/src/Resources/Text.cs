using System.Collections.Generic;
using Newtonsoft.Json;
using Omnigage.Runtime;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/texts` - https://omnigage.docs.apiary.io/#reference/text-resources/text-collection/retrieve-email-messages
    /// </summary>
    public class TextResource : Adapter
    {
        public override string Type { get; } = "texts";

        public string From { get; set; }

        public string To { get; set; }

        public string Status { get; set; }

        public string Direction { get; set; }

        public string Callback { get; set; }

        [JsonProperty(propertyName: "meta_prop")]
        public Dictionary<string, string> Meta;

        [JsonProperty(propertyName: "text-message")]
        public TextMessageResource TextMessage;

        [JsonProperty(propertyName: "phone-number")]
        public PhoneNumberResource PhoneNumber;

        public override string Serialize()
        {
            string serialized = base.Serialize();

            return serialized.Replace("meta_prop", "meta");
        }
    }
}
