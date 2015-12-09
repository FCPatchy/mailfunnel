using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mailfunnel.SMTP.Network
{
    public interface INetworkMessager
    {
        event EventHandler<NetworkClientConnectedEventArgs> ClientConnected;
        event EventHandler<NetworkClientMessageReceivedEventArgs> ClientMessageReceived;
        event EventHandler<NetworkClientDisconnectedEventArgs> ClientDisconnected;
        Task HandleClientAsync(ITcpClientAdapter client, int connections, CancellationToken token);
        Task SendMessage(ITcpClientAdapter client, CancellationToken token, string message);
        void DisconnectClient(ITcpClientAdapter tcpClient);
    }
}