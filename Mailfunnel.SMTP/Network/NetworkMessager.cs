using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mailfunnel.SMTP.Network
{
    public class NetworkMessager : INetworkMessager
    {
        public event EventHandler<NetworkClientConnectedEventArgs> ClientConnected;
        public event EventHandler<NetworkClientMessageReceivedEventArgs> ClientMessageReceived;
        public event EventHandler<NetworkClientDisconnectedEventArgs> ClientDisconnected;

        public async Task HandleClientAsync(ITcpClient client, int connections, CancellationToken token)
        {
            using (client)
            {
                if (ClientConnected != null)
                    ClientConnected(this, new NetworkClientConnectedEventArgs(client, token));

                var buf = new byte[4096];
                var stream = client.GetStream();

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

                    var messageText = Encoding.UTF8.GetString(resultBytes).TrimEnd();

                    if (messageText.Length > 0 && ClientMessageReceived != null)
                        ClientMessageReceived(this,
                            new NetworkClientMessageReceivedEventArgs(client, token, messageText));
                }
            }
        }

        public async Task SendMessage(ITcpClient client, CancellationToken token, string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            await client.GetStream().WriteAsync(bytes, 0, bytes.Length, token).ConfigureAwait(false);
        }

        public void DisconnectClient(ITcpClient tcpClient)
        {
            var clientIdentifier = tcpClient.ClientIdentifier;
            tcpClient.Close();

            if (ClientDisconnected != null)
                ClientDisconnected(this, new NetworkClientDisconnectedEventArgs(clientIdentifier));
        }
    }
}