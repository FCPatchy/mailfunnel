using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mailfunnel.SMTP.Logging;

namespace Mailfunnel.SMTP.Network
{
    public class NetworkMessager : INetworkMessager
    {
        private readonly ILogger _logger;
        public event EventHandler<NetworkClientConnectedEventArgs> ClientConnected;
        public event EventHandler<NetworkClientMessageReceivedEventArgs> ClientMessageReceived;
        public event EventHandler<NetworkClientDisconnectedEventArgs> ClientDisconnected;

        public NetworkMessager(ILogger logger)
        {
            _logger = logger;
        }

        public async Task HandleClientAsync(ITcpClientAdapter client, int connections, CancellationToken token)
        {
            using (client)
            {
                if (ClientConnected != null)
                    ClientConnected(this, new NetworkClientConnectedEventArgs(client, token));

                var buf = new byte[4096];

                using (var stream = client.GetStream())
                {
                    while (!token.IsCancellationRequested)
                    {
                        Array.Clear(buf, 0, buf.Length);
                        var timeoutTask = Task.Delay(TimeSpan.FromSeconds(15));

                        var amountReadTask = stream.ReadAsync(buf, 0, buf.Length, token);
                        var completedTask = await Task.WhenAny(timeoutTask, amountReadTask).ConfigureAwait(false);

                        // Client timed out
                        if (completedTask == timeoutTask)
                        {
                            if (ClientDisconnected != null)
                                ClientDisconnected(this, new NetworkClientDisconnectedEventArgs(client.ClientIdentifier));

                            break;
                        }

                        var amountRead = amountReadTask.Result;
                        if (amountRead == 0) break; // End of stream

                        var resultBytes = new byte[amountRead];
                        Array.Copy(buf, resultBytes, amountRead);

                        if (ClientMessageReceived != null)
                            ClientMessageReceived(this, new NetworkClientMessageReceivedEventArgs(client, token, resultBytes));
                    }
                }
            }
        }

        public async Task SendMessage(ITcpClientAdapter client, CancellationToken token, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            _logger.LogFormat("SERVER: {0}", message);

            await client.GetStream().WriteAsync(bytes, 0, bytes.Length, token).ConfigureAwait(false);
        }

        public void DisconnectClient(ITcpClientAdapter tcpClient)
        {
            var clientIdentifier = tcpClient.ClientIdentifier;
            tcpClient.Close();

            if (ClientDisconnected != null)
                ClientDisconnected(this, new NetworkClientDisconnectedEventArgs(clientIdentifier));
        }
    }
}