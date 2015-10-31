using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Mailfunnel.SMTP.Contracts;

namespace Mailfunnel.SMTP
{
    public class NetworkTcpListener : ITcpListenerAdapter
    {
        private readonly TcpListener _tcpListener;

        public NetworkTcpListener(IPAddress ipAddress, int port)
        {
            _tcpListener = new TcpListener(ipAddress, port);
        }

        public void Start()
        {
            _tcpListener.Start();
        }

        public void Stop()
        {
            _tcpListener.Stop();
        }

        public async Task<ITcpClient> AcceptTcpClientAsync()
        {
            var tcpClient = await _tcpListener.AcceptTcpClientAsync();

            return new TcpClientAdapter(tcpClient);
        }
    }
}
