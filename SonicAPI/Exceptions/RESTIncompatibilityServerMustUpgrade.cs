using System;

namespace SonicAPI.Exceptions
{
    public class RESTIncompatibilityServerMustUpgrade : ASonicApiException
    {
        public RESTIncompatibilityServerMustUpgrade() { }

        public RESTIncompatibilityServerMustUpgrade(string desc)
            : base(desc) { }

        public RESTIncompatibilityServerMustUpgrade(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}