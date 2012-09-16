using System;

namespace SonicAPI.Exceptions
{
    public class UserNotAuthorizedForOperation : ASonicApiException
    {
        public UserNotAuthorizedForOperation() { }

        public UserNotAuthorizedForOperation(string desc)
            : base(desc) { }

        public UserNotAuthorizedForOperation(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}