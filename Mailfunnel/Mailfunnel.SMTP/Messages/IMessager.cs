using System;
using Mailfunnel.SMTP.Clients;

namespace Mailfunnel.SMTP.Messages
{
    public interface IMessager
    {
        event EventHandler<ClientConnectedEventArgs> ClientConnected;
        event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;
        void SendMessage(Client client, Message greeting);
    }
}