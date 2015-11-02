using System.Threading;
using Mailfunnel.SMTP.Network;

namespace Mailfunnel.SMTP.Clients
{
    public class Client
    {
        public ClientManager.State ClientState { get; set; }
        public ITcpClient TcpClient { get; set; }
        public CancellationToken CancellationToken { get; set; }
        public EmailMessage Message { get; set; }
    }
}