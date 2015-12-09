using System.Threading;
using Mailfunnel.SMTP.Network;

namespace Mailfunnel.SMTP.Messages
{
    public class ClientMessageReceivedEventArgs
    {
        public ClientMessageReceivedEventArgs(ITcpClientAdapter client, CancellationToken cancellationToken,
            ClientMessage clientMessage)
        {
            Client = client;
            CancellationToken = cancellationToken;
            ClientMessage = clientMessage;
        }

        public CancellationToken CancellationToken { get; private set; }
        public ITcpClientAdapter Client { get; private set; }
        public ClientMessage ClientMessage { get; private set; }
    }
}