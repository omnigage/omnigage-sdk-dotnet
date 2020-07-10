using System.Collections.Generic;
using Newtonsoft.Json;
using Omnigage.Core;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/emails` - https://omnigage.docs.apiary.io/#reference/email-resources/email-collection/retrieve-file
    /// </summary>
    public class EmailResource : Adapter
    {
        public override string Type { get; } = "emails";

        public string From { get; set; }

        public string To { get; set; }

        public string Status { get; set; }

        public string Direction { get; set; }

        public string Callback { get; set; }

        [JsonProperty(propertyName: "meta_prop")]
        public Dictionary<string, string> Meta;

        [JsonProperty(propertyName: "email-message")]
        public EmailMessageResource EmailMessage;

        [JsonProperty(propertyName: "email-id")]
        public EmailIdResource EmailId;

        public override string Serialize()
        {
            string serialized = base.Serialize();

            return serialized.Replace("meta_prop", "meta");
        }
    }
}
