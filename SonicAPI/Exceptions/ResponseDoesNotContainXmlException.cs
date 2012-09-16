using System;

namespace SonicAPI.Exceptions
{
    public class ResponseDoesNotContainXmlException : ASonicApiException
    {
        public ResponseDoesNotContainXmlException() { }

        public ResponseDoesNotContainXmlException(string desc)
            : base(desc) { }

        public ResponseDoesNotContainXmlException(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}
