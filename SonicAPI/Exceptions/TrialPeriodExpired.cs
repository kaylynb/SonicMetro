using System;

namespace SonicAPI.Exceptions
{
    public class TrialPeriodExpired : ASonicApiException
    {
        public TrialPeriodExpired() { }

        public TrialPeriodExpired(string desc)
            : base(desc) { }

        public TrialPeriodExpired(string desc, Exception innerException)
            : base(desc, innerException) { }
    }
}