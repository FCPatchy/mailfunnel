using System.Net.Sockets;
using System.Threading;

namespace Mailfunnel.SMTP
{
    public interface IMessageSender
    {
        void SendMessage(NetworkStream stream, CancellationToken token, Message greeting);
    }
}