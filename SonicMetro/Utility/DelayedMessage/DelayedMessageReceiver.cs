using System;
using GalaSoft.MvvmLight.Messaging;

namespace SonicMetro.Utility.DelayedMessage
{
    public class DelayedMessageReceiver<T>
        where T : class
    {
        public DelayedMessageReceiver(Action<T> receivedMessageFunc)
        {
            Messenger.Default.Register<DelayedMessagePackage<T>>(this,
                x =>
                    {
                        if (x.DoesMessageExist)
                            receivedMessageFunc(x.Data);
                    });

            Messenger.Default.Send(new DelayedMessageRequest<T>());
        }
    }
}