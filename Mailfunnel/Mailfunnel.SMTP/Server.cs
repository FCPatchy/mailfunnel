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
        private readonly IClientManager _clientManager;

        public Server(Lazy<ITcpListenerAdapter> tcpListener, IMailCommunicator mailCommunicator, IClientManager clientManager)
        {
            _tcpListener = tcpListener;
            _mailCommunicator = mailCommunicator;
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

                await _mailCommunicator.HandleClientAsync(client, connections, token);
            }
        }
    }
}
