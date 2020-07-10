using System.Collections.Generic;
using Newtonsoft.Json;
using Omnigage.Runtime;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/email-templates` - https://omnigage.docs.apiary.io/#reference/email-resources/email-template
    /// </summary>
    public class EmailTemplateResource : Adapter
    {
        public override string Type { get; } = "email-templates";

        public string Subject { get; set; }

        public string Body { get; set; }

        [JsonProperty(propertyName: "upload-files")]
        public List<UploadResource> UploadFiles { get; set; }

        [JsonProperty(propertyName: "upload-attachments")]
        public List<UploadResource> UploadAttachments { get; set; }

        public List<FileResource> Files { get; set; }

        public List<FileResource> Attachments { get; set; }
    }
}