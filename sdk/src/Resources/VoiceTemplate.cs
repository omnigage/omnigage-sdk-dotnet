using Newtonsoft.Json;
using Omnigage.Runtime;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/voice-templates` - https://omnigage.docs.apiary.io/#reference/call-resources/voice-template
    /// </summary>
    public class VoiceTemplateResource : Adapter
    {
        public override string Type { get; } = "voice-templates";

        public string Name { get; set; }

        public string Kind { get; set; }

        public UploadResource Upload { get; set; }

        [JsonIgnore]
        public string FilePath { get; set; }
    }
}
