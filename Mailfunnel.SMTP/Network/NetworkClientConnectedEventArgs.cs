using System;
using System.Threading;

namespace Mailfunnel.SMTP.Network
{
    public class NetworkClientConnectedEventArgs : EventArgs
    {
        public NetworkClientConnectedEventArgs(ITcpClient client, CancellationToken cancellationToken)
        {
            Client = client;
            CancellationToken = cancellationToken;
        }

        public ITcpClient Client { get; private set; }
        public CancellationToken CancellationToken { get; set; }
    }
}