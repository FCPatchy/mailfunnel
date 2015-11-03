using System;
using System.IO;

namespace Mailfunnel.SMTP.Network
{
    public interface ITcpClient : IDisposable
    {
        int ClientIdentifier { get; }
        Stream GetStream();
        void Close();
    }
}