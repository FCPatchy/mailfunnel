using System;
using System.Text;
using Mailfunnel.SMTP.Clients;
using Mailfunnel.SMTP.Logging;
using Mailfunnel.SMTP.Network;

namespace Mailfunnel.SMTP.Messages
{
    public class Messager : IMessager
    {
        private readonly IMessageProcessor _messageProcessor;
        private readonly INetworkMessager _networkMessager;
        private readonly ILogger _logger;

        public Messager(INetworkMessager networkMessager, IMessageProcessor messageProcessor, ILogger logger)
        {
            _networkMessager = networkMessager;
            _messageProcessor = messageProcessor;
            _logger = logger;

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
                var messageText = Encoding.UTF8.GetString(e.Message).TrimEnd();

                var clientMessage = _messageProcessor.ProcessMessage(messageText);

                _logger.LogFormat("CLIENT: {0}", messageText);

                if (messageText.Length > 0 && ClientMessageReceived != null)
                    ClientMessageReceived(sender, new ClientMessageReceivedEventArgs(e.Client, e.CancellationToken, clientMessage));
            }
        }
    }
}