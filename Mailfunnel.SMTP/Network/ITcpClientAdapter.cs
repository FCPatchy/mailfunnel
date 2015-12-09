using System;
using System.Net.Sockets;

namespace Mailfunnel.SMTP.Network
{
    public interface ITcpClientAdapter : IDisposable
    {
        int ClientIdentifier { get; }
        bool IsClosed { get; }
        NetworkStream GetStream();
        void Close();
    }
}