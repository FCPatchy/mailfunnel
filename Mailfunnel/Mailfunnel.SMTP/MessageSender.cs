using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mailfunnel.SMTP
{
    public class MessageSender : IMessageSender
    {
        public async void SendMessage(NetworkStream stream, CancellationToken token, Message message)
        {
            switch (message)
            {
                case Message.Greeting:
                    await SendGreeting(stream, token);
                    break;

                case Message.EHLO:
                    await SendEHLO(stream, token);
                    break;

                    case Message.OK:
                    await SendOK(stream, token);
                    break;
            }
            
        }

        private static async Task SendGreeting(NetworkStream stream, CancellationToken token)
        {
            var ehloBytes = Encoding.ASCII.GetBytes("220 Mailfunnel\r\n");
            await stream.WriteAsync(ehloBytes, 0, ehloBytes.Length, token).ConfigureAwait(false);
            Console.WriteLine("Server: 220 Mailfunnel");
        }

        private static async Task SendEHLO(NetworkStream stream, CancellationToken token)
        {
            var ehloBytes = Encoding.ASCII.GetBytes("250 EHLO\r\n");
            await stream.WriteAsync(ehloBytes, 0, ehloBytes.Length, token).ConfigureAwait(false);
            Console.WriteLine("Server: 250 EHLO");
        }

        private async Task SendOK(NetworkStream stream, CancellationToken token)
        {
            var ehloBytes = Encoding.ASCII.GetBytes("250\r\n");
            await stream.WriteAsync(ehloBytes, 0, ehloBytes.Length, token).ConfigureAwait(false);
            Console.WriteLine("Server: 250");
        }
    }
}
