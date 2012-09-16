using System;

namespace SonicAPI.Exceptions
{
    public abstract class ASonicApiException : Exception
    {
        protected ASonicApiException() { }

        protected ASonicApiException(string desc)
            : base(desc) { }

        protected ASonicApiException(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}
