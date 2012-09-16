using System;

namespace SonicAPI.Exceptions
{
    public class RESTIncompatibilityClientMustUpgrade : ASonicApiException
    {
        public RESTIncompatibilityClientMustUpgrade() { }

        public RESTIncompatibilityClientMustUpgrade(string desc)
            : base(desc) { }

        public RESTIncompatibilityClientMustUpgrade(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}