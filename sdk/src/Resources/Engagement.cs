using Omnigage.Util;

namespace Omnigage.Resources
{
    /// <summary>
    /// Resource `/engagements` - https://omnigage.docs.apiary.io/#reference/engagement-resources
    /// </summary>
    public class EngagementResource : Adapter
    {
        public override string Type { get; } = "engagements";

        public string Name;

        public string Direction;

        public string Status;
    }
}
