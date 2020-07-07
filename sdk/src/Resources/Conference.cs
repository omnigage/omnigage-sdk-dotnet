using Omnigage.Util;

namespace Omnigage.Resources
{
    /// <summary>
    /// Resource: `/conferences` - https://omnigage.docs.apiary.io/#reference/phone-resources/conference-collection
    /// </summary>
    public class ConferenceResource : Adapter
    {
        public override string Type { get; } = "conferences";
    }
}
