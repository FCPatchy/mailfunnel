using System;

namespace Mailfunnel.SMTP.Clients
{
    public interface IClientManager
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }

    public class MessageReceivedEventArgs
    {
        public MessageReceivedEventArgs(EmailMessage message)
        {
            Message = message;
        }

        public EmailMessage Message { get; private set; }
    }
}