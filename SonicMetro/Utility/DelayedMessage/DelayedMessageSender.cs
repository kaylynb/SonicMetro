using GalaSoft.MvvmLight.Messaging;

namespace SonicMetro.Utility.DelayedMessage
{
    public class DelayedMessageSender<T>
        where T : class
    {
        private T _data;

        public DelayedMessageSender()
        {
            Messenger.Default.Register<DelayedMessageRequest<T>>(
                this,
                x =>
                    {
                        if (_data != null)
                        {
                            Messenger.Default.Send(new DelayedMessagePackage<T> {DoesMessageExist = _data != null, Data = _data});
                        }
                    });
        }

        public void SetData(T data)
        {
            _data = data;
        }
    }
}
