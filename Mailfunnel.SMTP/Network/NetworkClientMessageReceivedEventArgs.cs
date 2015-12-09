using System;
using System.Threading;

namespace Mailfunnel.SMTP.Network
{
    public class NetworkClientMessageReceivedEventArgs : EventArgs
    {
        public NetworkClientMessageReceivedEventArgs(ITcpClientAdapter client, CancellationToken cancellationToken,
            string messageText)
        {
            Client = client;
            CancellationToken = cancellationToken;
            MessageText = messageText;
        }

        public ITcpClientAdapter Client { get; private set; }
        public CancellationToken CancellationToken { get; private set; }
        public string MessageText { get; private set; }
    }
}