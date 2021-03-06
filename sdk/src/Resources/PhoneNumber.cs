﻿using Omnigage.Runtime;

namespace Omnigage.Resource
{
    /// <summary>
    /// Resource: `/email-ids` - https://omnigage.docs.apiary.io/#reference/phone-resources/phone-number-collection/retrieve-phone-numbers
    /// </summary>
    public class PhoneNumberResource : Adapter
    {
        public override string Type { get; } = "phone-numbers";
    }
}
