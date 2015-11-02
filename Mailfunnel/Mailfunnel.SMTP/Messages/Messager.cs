﻿using System;
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
        }

        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;

        public void SendMessage(Client client, Message message)
        {
        }

        private void NetworkClientConnected(object sender, NetworkClientConnectedEventArgs e)
        {
            // The event is simply piped
            if (ClientConnected != null)
                ClientConnected(sender, new ClientConnectedEventArgs(e.Client, e.CancellationToken));
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