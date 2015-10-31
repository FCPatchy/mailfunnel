using System.Threading.Tasks;

namespace Mailfunnel.SMTP.Contracts
{
    public interface ITcpListenerAdapter
    {
        void Start();
        void Stop();
        Task<ITcpClient> AcceptTcpClientAsync();
    }
}
