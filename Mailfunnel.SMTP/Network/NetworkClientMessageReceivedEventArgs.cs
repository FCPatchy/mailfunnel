using System;
using System.Threading;

namespace Mailfunnel.SMTP.Network
{
    public class NetworkClientMessageReceivedEventArgs : EventArgs
    {
        public NetworkClientMessageReceivedEventArgs(ITcpClientAdapter client, CancellationToken cancellationToken, byte[] message)
        {
            Client = client;
            CancellationToken = cancellationToken;
            Message = message;
        }

        public ITcpClientAdapter Client { get; private set; }
        public CancellationToken CancellationToken { get; private set; }
        public byte[] Message { get; private set; }
    }
}