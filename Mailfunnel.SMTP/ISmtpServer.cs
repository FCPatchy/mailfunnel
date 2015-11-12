using System;
using Mailfunnel.SMTP.Clients;

namespace Mailfunnel.SMTP
{
    public interface ISmtpServer
    {
        void Listen();
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}