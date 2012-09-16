using System;

namespace SonicAPI.Exceptions
{
    public class DataNotFound : ASonicApiException
    {
        public DataNotFound() { }

        public DataNotFound(string desc)
            : base(desc) { }

        public DataNotFound(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}