using Newtonsoft.Json;
using Omnigage.Util;

namespace Omnigage.Resources
{
    /// <summary>
    /// Resource: `/activities` - https://omnigage.docs.apiary.io/#reference/engagement-resources/activity-collection
    /// </summary>
    public class ActivityResource : Adapter
    {
        public override string Type { get; } = "activities";

        public string Name;

        public string Kind;

        public EngagementResource Engagement;

        [JsonProperty(propertyName: "caller-id")]
        public CallerIdResource CallerId;
    }
}
