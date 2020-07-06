using Omnigage.Util;

namespace Omnigage.Resources
{
    /// <summary>
    /// Resource: `/email-ids` - https://omnigage.docs.apiary.io/#reference/identity-resources/email-id-collection/retrieve-email-messages
    /// </summary>
    public class EmailIdResource : Adapter
    {
        public override string Type { get; } = "email-ids";
    }
}
