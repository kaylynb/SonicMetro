using System;

namespace SonicAPI.Exceptions
{
    public class WrongUsernameOrPassword : ASonicApiException
    {
        public WrongUsernameOrPassword() { }

        public WrongUsernameOrPassword(string desc)
            : base(desc) { }

        public WrongUsernameOrPassword(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}