using System;
using System.Threading;

namespace Mailfunnel.SMTP.Network
{
    public class NetworkClientConnectedEventArgs : EventArgs
    {
        public NetworkClientConnectedEventArgs(ITcpClientAdapter client, CancellationToken cancellationToken)
        {
            Client = client;
            CancellationToken = cancellationToken;
        }

        public ITcpClientAdapter Client { get; private set; }
        public CancellationToken CancellationToken { get; set; }
    }
}