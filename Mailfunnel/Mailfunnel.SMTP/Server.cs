using System;
using System.Threading;
using System.Threading.Tasks;
using Mailfunnel.SMTP.Contracts;

namespace Mailfunnel.SMTP
{
    public class Server : IServer
    {
        private readonly Lazy<ITcpListenerAdapter> _tcpListener;
        private readonly IMailCommunicator _mailCommunicator;

        public Server(Lazy<ITcpListenerAdapter> tcpListener, IMailCommunicator mailCommunicator)
        {
            _tcpListener = tcpListener;
            _mailCommunicator = mailCommunicator;
        }

        public async void Listen()
        {
            var cts = new CancellationTokenSource();

            var tcpListener = _tcpListener.Value;

            try
            {
                tcpListener.Start();
                
                    AcceptConnectionsAsync(tcpListener, cts.Token);

                while (true)
                {

                }
            }
            finally
            {
                cts.Cancel();
                tcpListener.Stop();
            }
        }

        private async Task AcceptConnectionsAsync(ITcpListenerAdapter listener, CancellationToken token)
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
