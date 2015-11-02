using System.Threading;
using Mailfunnel.SMTP.Network;

namespace Mailfunnel.SMTP.Messages
{
    public class ClientConnectedEventArgs
    {
        public ClientConnectedEventArgs(ITcpClient client, CancellationToken cancellationToken)
        {
            Client = client;
            CancellationToken = cancellationToken;
        }

        public ITcpClient Client { get; private set; }
        public CancellationToken CancellationToken { get; private set; }
    }
}