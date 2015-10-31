using System.Threading;
using System.Threading.Tasks;

namespace Mailfunnel.SMTP.Contracts
{
    public interface IMailCommunicator
    {
        Task HandleClientAsync(ITcpClient client, int connections, CancellationToken token);
    }
}
