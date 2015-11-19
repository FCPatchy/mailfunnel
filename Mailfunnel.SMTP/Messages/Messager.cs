using System;
using Mailfunnel.SMTP.Clients;
using Mailfunnel.SMTP.Network;

namespace Mailfunnel.SMTP.Messages
{
    public class Messager : IMessager
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly INetworkMessager _networkMessager;

        public Messager(INetworkMessager networkMessager, IMessageProcessor messageProcessor)
        {
            _networkMessager = networkMessager;
            _messageProcessor = messageProcessor;

            _networkMessager.ClientConnected += NetworkClientConnected;
            _networkMessager.ClientMessageReceived += NetworkClientMessageReceived;
            _networkMessager.ClientDisconnected += NetworkClientDisconnected;
        }

        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;
        public event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;

        public void SendMessage(Client client, IOutboundMessage outboundMessage)
        {
            var generatedMessage = _messageProcessor.GenerateMessage(outboundMessage);
            if (outboundMessage.Multiline)
            {
                // Don't send the message yet - store it in the message buffer
                client.MessageBuffer += generatedMessage;
            }
            else
            {
                if (!string.IsNullOrEmpty(client.MessageBuffer))
                {
                    generatedMessage = client.MessageBuffer + generatedMessage;
                    client.MessageBuffer = string.Empty;
                }

                _networkMessager.SendMessage(client.TcpClient, client.CancellationToken, generatedMessage);
            }
        }

        public void DisconnectClient(Client client)
        {
            _networkMessager.DisconnectClient(client.TcpClient);
        }

        private void NetworkClientConnected(object sender, NetworkClientConnectedEventArgs e)
        {
            // The event is simply piped
            if (ClientConnected != null)
                ClientConnected(sender, new ClientConnectedEventArgs(e.Client, e.CancellationToken));
        }

        private void NetworkClientDisconnected(object sender, NetworkClientDisconnectedEventArgs e)
        {
            // The event is simply piped
            if (ClientDisconnected != null)
                ClientDisconnected(sender, new ClientDisconnectedEventArgs(e.ClientHashCode));
        }

        private void NetworkClientMessageReceived(object sender, NetworkClientMessageReceivedEventArgs e)
        {
            if (ClientMessageReceived != null)
            {
                var clientMessage = _messageProcessor.ProcessMessage(e.MessageText);

                ClientMessageReceived(sender,
                    new ClientMessageReceivedEventArgs(e.Client, e.CancellationToken, clientMessage));
            }
        }
    }
}