using System.Threading;
using Mailfunnel.SMTP.Network;

namespace Mailfunnel.SMTP.Messages
{
    public class ClientMessageReceivedEventArgs
    {
        public ClientMessageReceivedEventArgs(ITcpClient client, CancellationToken cancellationToken,
            ClientMessage clientMessage)
        {
            Client = client;
            CancellationToken = cancellationToken;
            ClientMessage = clientMessage;
        }

        public CancellationToken CancellationToken { get; private set; }
        public ITcpClient Client { get; private set; }
        public ClientMessage ClientMessage { get; private set; }
    }
}