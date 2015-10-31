using System;
using System.IO;

namespace Mailfunnel.SMTP.Contracts
{
    public interface ITcpClient : IDisposable
    {
        Stream GetStream();
    }
}
