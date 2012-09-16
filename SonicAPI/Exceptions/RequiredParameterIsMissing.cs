using System;

namespace SonicAPI.Exceptions
{
    public class RequiredParameterIsMissing : ASonicApiException
    {
        public RequiredParameterIsMissing() { }

        public RequiredParameterIsMissing(string desc)
            : base(desc) { }

        public RequiredParameterIsMissing(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}