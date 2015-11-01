using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mailfunnel.SMTP.Contracts;

namespace Mailfunnel.SMTP
{
    public class MessageSender : IMessageSender
    {
        public async void SendMessage(Stream stream, CancellationToken token, Message message)
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

                case Message.SendData:
                    await SendData(stream, token);
                    break;

                case Message.BadSequence:
                    await SendBadSequence(stream, token);
                    break;
            }
            
        }

        private static async Task SendGreeting(Stream stream, CancellationToken token)
        {
            var ehloBytes = Encoding.ASCII.GetBytes("220 Mailfunnel\r\n");
            await stream.WriteAsync(ehloBytes, 0, ehloBytes.Length, token).ConfigureAwait(false);
            Console.WriteLine("Server: 220 Mailfunnel");
        }

        private static async Task SendEHLO(Stream stream, CancellationToken token)
        {
            var ehloBytes = Encoding.ASCII.GetBytes("250 EHLO\r\n");
            await stream.WriteAsync(ehloBytes, 0, ehloBytes.Length, token).ConfigureAwait(false);
            Console.WriteLine("Server: 250 EHLO");
        }

        private async Task SendOK(Stream stream, CancellationToken token)
        {
            var ehloBytes = Encoding.ASCII.GetBytes("250 OK\r\n");
            await stream.WriteAsync(ehloBytes, 0, ehloBytes.Length, token).ConfigureAwait(false);
            Console.WriteLine("Server: 250 OK");
        }

        private async Task SendData(Stream stream, CancellationToken token)
        {
            var ehloBytes = Encoding.ASCII.GetBytes("354 Start mail input; end with <CRLF>.<CRLF>\r\n");
            await stream.WriteAsync(ehloBytes, 0, ehloBytes.Length, token).ConfigureAwait(false);
            Console.WriteLine("Server: 354 Start mail input; end with <CRLF>.<CRLF>");
        }

        private async Task SendBadSequence(Stream stream, CancellationToken token)
        {
            var ehloBytes = Encoding.ASCII.GetBytes("503 Bad sequence of commands\r\n");
            await stream.WriteAsync(ehloBytes, 0, ehloBytes.Length, token).ConfigureAwait(false);
            Console.WriteLine("Server: 503 Bad sequence of commands");
        }
    }
}
