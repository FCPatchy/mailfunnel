using System;
using System.Collections.Generic;
using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.MIME;

namespace Mailfunnel.SMTP.Clients
{
    public class ClientManager : IClientManager
    {
        private readonly Dictionary<int, Client> _clients;
        private readonly IMessager _messager;
        private readonly IMimeParser _mimeParser;

        public ClientManager(IMessager messager, IMimeParser mimeParser)
        {
            _messager = messager;
            _mimeParser = mimeParser;

            _clients = new Dictionary<int, Client>();

            // Subscribe to messager events
            _messager.ClientConnected += OnClientConnected;
            _messager.ClientMessageReceived += OnClientMessageReceived;
            _messager.ClientDisconnected += OnClientDisconnected;
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        

        private void AuthRequired(Client client, string s)
        {
            _messager.SendMessage(client, new OutboundMessageAuthRequired());
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            var client = new Client(args.Client, args.CancellationToken, _messager, _mimeParser);

            _clients.Add(client.ClientIdentifier, client);
        }

        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            _clients.Remove(args.ClientIdentifier);
        }

        private void OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            var clientIdentifier = e.Client.ClientIdentifier;
            var client = _clients[clientIdentifier];

            client.MessageReceived(e.ClientMessage);
        }
    }
}