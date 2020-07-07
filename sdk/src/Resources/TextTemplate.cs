using System.Collections.Generic;
using Newtonsoft.Json;
using Omnigage.Util;

namespace Omnigage.Resources
{
    /// <summary>
    /// Resource: `/text-templates` - https://omnigage.docs.apiary.io/#reference/text-resources/text-template-collection/retrieve-text-templates
    /// </summary>
    public class TextTemplateResource : Adapter
    {
        public override string Type { get; } = "text-templates";

        public string Name { get; set; }

        public string Body { get; set; }

        public List<UploadResource> Uploads { get; set; }

        public List<FileResource> Files { get; set; }
    }
}