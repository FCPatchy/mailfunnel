using System;

namespace Mailfunnel.SMTP
{
    public interface IClientManager
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }

    public class MessageReceivedEventArgs
    {
        public EmailMessage Message { get; private set; }

        public MessageReceivedEventArgs(EmailMessage message)
        {
            Message = message;
        }
    }
}
