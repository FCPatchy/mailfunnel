using System;
using System.Net.Sockets;

namespace Mailfunnel.SMTP.Network
{
    public class TcpClientAdapter : ITcpClientAdapter
    {
        private bool disposed = false;
        private TcpClient _tcpClient;

        public TcpClientAdapter(int clientIdentifier, TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            ClientIdentifier = clientIdentifier;
        }

        public int ClientIdentifier { get; }
        public bool IsClosed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (_tcpClient != null)
                    {
                        _tcpClient.Close();
                        _tcpClient = null;
                    }
                }
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public NetworkStream GetStream()
        {
            return _tcpClient.GetStream();
        }

        public void Close()
        {
            _tcpClient.Close();
            IsClosed = true;
        }
    }
}