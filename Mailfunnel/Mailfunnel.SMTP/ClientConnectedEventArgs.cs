using System;
using System.Threading;
using Mailfunnel.SMTP.Contracts;

namespace Mailfunnel.SMTP
{
    public class ClientConnectedEventArgs : EventArgs
    {
        public ITcpClient Client { get; private set; }
        public CancellationToken CancellationToken { get; set; }

        public ClientConnectedEventArgs(ITcpClient client, CancellationToken cancellationToken)
        {
            Client = client;
            CancellationToken = cancellationToken;
        }
    }
}
