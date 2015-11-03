using System;
using System.IO;
using System.Net.Sockets;

namespace Mailfunnel.SMTP.Network
{
    public class TcpClientAdapter : ITcpClient
    {
        private readonly TcpClient _tcpClient;

        public int ClientIdentifier { get; }

        public TcpClientAdapter(int clientIdentifier, TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            ClientIdentifier = clientIdentifier;
        }

        public void Dispose()
        {
            ((IDisposable) _tcpClient).Dispose();
        }

        public Stream GetStream()
        {
            return _tcpClient.GetStream();
        }

        public void Close()
        {
            _tcpClient.Close();
        }
    }
}