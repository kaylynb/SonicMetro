using System;

namespace SonicAPI.Exceptions
{
    public class UnknownError : ASonicApiException
    {
        public UnknownError() { }

        public UnknownError(string desc)
            : base(desc) { }

        public UnknownError(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}