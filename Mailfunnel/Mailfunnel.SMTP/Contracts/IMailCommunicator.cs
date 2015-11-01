using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mailfunnel.SMTP.Contracts
{
    public interface IMailCommunicator
    {
        event EventHandler<ClientConnectedEventArgs> ClientConnected;
        event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;
        Task HandleClientAsync(ITcpClient client, int connections, CancellationToken token);
        void SendMessage(ITcpClient client, CancellationToken token, Message message);
    }
}
