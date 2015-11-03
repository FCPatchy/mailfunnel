using System;
using System.Threading;
using System.Threading.Tasks;
using Mailfunnel.SMTP.Clients;
using Mailfunnel.SMTP.Network;

namespace Mailfunnel.SMTP
{
    public class Server : IServer
    {
        private readonly IClientManager _clientManager;
        private readonly INetworkMessager _networkMessager;
        private readonly Lazy<ITcpListenerAdapter> _tcpListener;

        public Server(Lazy<ITcpListenerAdapter> tcpListener, INetworkMessager networkMessager, IClientManager clientManager)
        {
            _tcpListener = tcpListener;
            _networkMessager = networkMessager;
            _clientManager = clientManager;

            _clientManager.MessageReceived += (sender, args) =>
            {
                Console.WriteLine("We have a message!!!!");
                var y = args.Message;
            };
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

                await _networkMessager.HandleClientAsync(client, connections, token);
            }
        }
    }
}