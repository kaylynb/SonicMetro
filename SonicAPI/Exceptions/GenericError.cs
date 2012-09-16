using System;

namespace SonicAPI.Exceptions
{
    public class GenericError : ASonicApiException
    {
        public GenericError() { }

        public GenericError(string desc)
            : base(desc) { }

        public GenericError(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}
