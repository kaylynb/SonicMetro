using GalaSoft.MvvmLight.Messaging;

namespace SonicMetro.Utility.DelayedMessage
{
    public class DelayedMessagePackage<T> : MessageBase
    {
        public bool DoesMessageExist { get; set; }
        public T Data { get; set; }
    }
}
