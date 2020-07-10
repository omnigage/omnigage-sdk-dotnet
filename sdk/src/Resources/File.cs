using Newtonsoft.Json;
using Omnigage.Core;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/files` - https://omnigage.docs.apiary.io/#reference/media-resources/file
    /// </summary>
    public class FileResource : Adapter
    {
        public override string Type { get; } = "files";

        public string Url;

        [JsonProperty(propertyName: "size-in-bytes")]
        public int SizeInBytes;

        [JsonProperty(propertyName: "content-type")]
        public string ContentType;

        public int Width;

        public int Height;
    }
}
