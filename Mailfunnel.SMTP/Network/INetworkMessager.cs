using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mailfunnel.SMTP.Network
{
    public interface INetworkMessager
    {
        event EventHandler<NetworkClientConnectedEventArgs> ClientConnected;
        event EventHandler<NetworkClientMessageReceivedEventArgs> ClientMessageReceived;
        Task HandleClientAsync(ITcpClient client, int connections, CancellationToken token);
        Task SendMessage(ITcpClient client, CancellationToken token, string message);
    }
}