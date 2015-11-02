using System;
using System.IO;

namespace Mailfunnel.SMTP.Network
{
    public interface ITcpClient : IDisposable
    {
        Stream GetStream();
    }
}