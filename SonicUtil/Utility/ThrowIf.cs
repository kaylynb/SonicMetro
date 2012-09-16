using System;

namespace SonicUtil.Utility
{
    public static class ThrowIf
    {
        public static void Null<T>(T obj, string paramName)
            where T : class
        {
            if(obj == null)
                throw new ArgumentNullException(paramName ?? "");
        }

        public static void NullOrEmpty(string str, string paramName)
        {
            if(string.IsNullOrEmpty(str))
                throw new ArgumentNullException(paramName ?? "");
        }
    }
}