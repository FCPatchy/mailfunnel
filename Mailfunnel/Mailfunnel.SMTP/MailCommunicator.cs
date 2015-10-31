using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mailfunnel.SMTP.Contracts;

namespace Mailfunnel.SMTP
{
    public class MailCommunicator : IMailCommunicator
    {
        private readonly IMessageSender _messageSender;
        private readonly IMessageProcessor _messageProcessor;

        public MailCommunicator(IMessageSender messageSender, IMessageProcessor messageProcessor)
        {
            Console.WriteLine("MailCommunicator init!");
            _messageSender = messageSender;
            _messageProcessor = messageProcessor;
        }

        public async Task HandleClientAsync(ITcpClient client, int connections, CancellationToken token)
        {
            using (client)
            {
                var buf = new byte[4096];
                var stream = client.GetStream();

                _messageSender.SendMessage(stream, token, Message.Greeting);

                while (!token.IsCancellationRequested)
                {
                    var timeoutTask = Task.Delay(TimeSpan.FromSeconds(15));
                    var amountReadTask = stream.ReadAsync(buf, 0, buf.Length, token);
                    var completedTask = await Task.WhenAny(timeoutTask, amountReadTask).ConfigureAwait(false);

                    // Client timed out

                    if (completedTask == timeoutTask)
                        break;

                    var amountRead = amountReadTask.Result;
                    if (amountRead == 0) break; // End of stream

                    var command = _messageProcessor.ProcessMessage(buf);

                    switch (command)
                    {
                        case SMTPCommand.EHLO:
                            _messageSender.SendMessage(stream, token, Message.EHLO);
                            Console.WriteLine("Replied to EHLO");
                            break;

                        case SMTPCommand.MAIL:
                            _messageSender.SendMessage(stream, token, Message.OK);
                            Console.WriteLine("Replied to MAIL FROM");
                            break;

                        case SMTPCommand.RCPT:
                            _messageSender.SendMessage(stream, token, Message.OK);
                            Console.WriteLine("Replied to RCPT TO");
                            break;

                            case SMTPCommand.DATA:
                            _messageSender.SendMessage(stream, token, Message.SendData);
                            Console.WriteLine("Replied to DATA");
                            break;

                        default:
                            var txt = Encoding.ASCII.GetString(buf);

                            Console.WriteLine("No idea what this means: " + txt);
                            break;
                    }

                    /*

                var str = Encoding.ASCII.GetString(buf);
                    Console.WriteLine(str);

                    if (str.Length == 0)
                    {
                        var ehlo = "220 Pleased to meet you\r\n";
                        Buffer.BlockCopy(ehlo.ToCharArray(), 0, buf, 0, ehlo.Length);
                    }

                    await stream.WriteAsync(buf, 0, amountRead > 0 ? amountRead : 4, token).ConfigureAwait(false);
                    */
                }
            }
        }
    }
}
