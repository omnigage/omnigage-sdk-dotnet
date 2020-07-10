using System.Collections.Generic;
using Newtonsoft.Json;
using Omnigage.Core;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/text-messages` - https://omnigage.docs.apiary.io/#reference/text-resources/text-message-collection/retrieve-text-messages
    /// </summary>
    public class TextMessageResource : Adapter
    {
        public override string Type { get; } = "text-messages";

        public string Body { get; set; }

        [JsonProperty(propertyName: "is-draft")]
        public bool IsDraft { get; set; }

        [JsonProperty(propertyName: "text-template")]
        public TextTemplateResource TextTemplate;

        public List<UploadResource> Uploads { get; set; }

        public List<FileResource> Files { get; set; }
    }
}
