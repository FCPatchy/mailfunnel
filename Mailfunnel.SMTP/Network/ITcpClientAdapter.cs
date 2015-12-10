using System;
using System.IO;

namespace Mailfunnel.SMTP.Network
{
    public interface ITcpClientAdapter : IDisposable
    {
        int ClientIdentifier { get; }
        bool IsClosed { get; }
        Stream GetStream();
        void Close();
    }
}