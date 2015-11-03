using System;
using Mailfunnel.SMTP.Clients;

namespace Mailfunnel.SMTP.Messages
{
    public interface IMessager
    {
        event EventHandler<ClientConnectedEventArgs> ClientConnected;
        event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;
        event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;

        void SendMessage(Client client, IOutboundMessage outboundMessage);
        void DisconnectClient(Client client);
    }
}