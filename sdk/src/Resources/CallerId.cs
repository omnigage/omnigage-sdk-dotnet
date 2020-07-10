using Omnigage.Core;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/caller-ids` - https://omnigage.docs.apiary.io/#reference/identity-resources/caller-id-collection
    /// </summary>
    public class CallerIdResource : Adapter
    {
        public override string Type { get; } = "caller-ids";
    }
}
