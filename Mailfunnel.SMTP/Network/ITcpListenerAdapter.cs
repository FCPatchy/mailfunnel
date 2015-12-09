using System.Threading.Tasks;

namespace Mailfunnel.SMTP.Network
{
    public interface ITcpListenerAdapter
    {
        void Start();
        void Stop();
        Task<ITcpClientAdapter> AcceptTcpClientAsync();
    }
}