using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Mailfunnel.SMTP
{
    public class Server : IServer
    {
        private readonly IMailCommunicator _mailCommunicator;

        public Server(IMailCommunicator mailCommunicator)
        {
            _mailCommunicator = mailCommunicator;
        }

        public async void Listen()
        {
            var cts = new CancellationTokenSource();
            var listener = new TcpListener(IPAddress.Any, 25);

            try
            {
                listener.Start();
                
                    AcceptConnectionsAsync(listener, cts.Token);

                while (true)
                {

                }
            }
            finally
            {
                cts.Cancel();
                listener.Stop();
            }
        }

        private async Task AcceptConnectionsAsync(TcpListener listener, CancellationToken token)
        {
            var connections = 0;

            while (!token.IsCancellationRequested)
            {
                var client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);

                connections++;

                await _mailCommunicator.HandleClientAsync(client, connections, token);
            }
        }
    }
}
