using System.IO;
using System.Threading;

namespace Mailfunnel.SMTP.Contracts
{
    public interface IMessageSender
    {
        void SendMessage(Stream stream, CancellationToken token, Message greeting);
    }
}