using System.IO;
using System.Net.Sockets;
using Mailfunnel.SMTP.Network;

namespace Mailfunnel.SMTP
{
    public class TcpClientAdapter : ITcpClient
    {
        private readonly TcpClient _tcpClient;

        public TcpClientAdapter(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }

        public void Dispose()
        {
            // _tcpClient.Dispose(); // todo
        }

        public Stream GetStream()
        {
            return _tcpClient.GetStream();
        }
    }
}