using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Mailfunnel.SMTP
{
    public interface IMailCommunicator
    {
        Task HandleClientAsync(TcpClient client, int connections, CancellationToken token);
    }
}
