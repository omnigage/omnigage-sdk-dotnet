using System.Collections.Generic;
using Newtonsoft.Json;
using JsonApiSerializer;
using Omnigage.Util;

namespace Omnigage.Resources
{
    /// <summary>
    /// Resource: `/envelopes` - https://omnigage.docs.apiary.io/#reference/engagement-resources/envelope-collection
    /// </summary>
    public class EnvelopeResource : Adapter
    {
        public override string Type { get; } = "envelopes";

        [JsonProperty(propertyName: "phone-number")]
        public string PhoneNumber;

        [JsonProperty(propertyName: "email-address")]
        public string EmailAddress;

        [JsonProperty(propertyName: "meta_prop")]
        public Dictionary<string, string> Meta;

        public EngagementResource Engagement;

        public static string SerializeBulk(List<EnvelopeResource> records)
        {
            string payload = JsonConvert.SerializeObject(records, new JsonApiSerializerSettings());
            // Work around `JsonApiSerializer` moving properties named "meta" above "attributes"
            return payload.Replace("meta_prop", "meta");
        }

        public override string Serialize()
        {
            string serialized = base.Serialize();

            return serialized.Replace("meta_prop", "meta");
        }
    }
}
