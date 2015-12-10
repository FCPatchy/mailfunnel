using System;
using System.Threading.Tasks;
using Mailfunnel.SMTP.Clients;

namespace Mailfunnel.SMTP
{
    public interface ISmtpServer
    {
        Task Listen();
        event EventHandler<MessageReceivedEventArgs> MessageReceived;
    }
}