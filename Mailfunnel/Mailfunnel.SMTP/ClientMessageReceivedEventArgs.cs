using System;
using Mailfunnel.SMTP.Contracts;

namespace Mailfunnel.SMTP
{
    public class ClientMessageReceivedEventArgs : EventArgs
    {
        public ITcpClient Client { get; private set; }
        public SMTPCommand Command { get; set; }
        public string MessageText { get; private set; }

        public ClientMessageReceivedEventArgs(ITcpClient client, SMTPCommand command, string messageText)
        {
            Client = client;
            Command = command;
            MessageText = messageText;
        }
    }
}