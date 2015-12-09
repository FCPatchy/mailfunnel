using System.Threading;
using Mailfunnel.SMTP.Network;

namespace Mailfunnel.SMTP.Messages
{
    public class ClientConnectedEventArgs
    {
        public ClientConnectedEventArgs(ITcpClientAdapter client, CancellationToken cancellationToken)
        {
            Client = client;
            CancellationToken = cancellationToken;
        }

        public ITcpClientAdapter Client { get; private set; }
        public CancellationToken CancellationToken { get; private set; }
    }
}