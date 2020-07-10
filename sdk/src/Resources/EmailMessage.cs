using System.Collections.Generic;
using Newtonsoft.Json;
using Omnigage.Runtime;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/email-messages` - https://omnigage.docs.apiary.io/#reference/email-resources/email-message-collection/retrieve-email-messages
    /// </summary>
    public class EmailMessageResource : Adapter
    {
        public override string Type { get; } = "email-messages";

        public string Subject { get; set; }

        public string Body { get; set; }

        [JsonProperty(propertyName: "is-draft")]
        public bool IsDraft { get; set; }

        [JsonProperty(propertyName: "email-template")]
        public EmailTemplateResource EmailTemplate;

        public List<UploadResource> Uploads { get; set; }

        public List<FileResource> Files { get; set; }
    }
}
