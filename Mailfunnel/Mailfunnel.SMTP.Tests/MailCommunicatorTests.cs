using System.IO;
using System.Text;
using System.Threading;
using Mailfunnel.SMTP.Contracts;
using NUnit.Framework;

namespace Mailfunnel.SMTP.Tests
{
    [TestFixture]
    public class MailCommunicatorTests
    {
        [Test]
        public async void HandleClientAsync_SendsWelcomeMessage()
        {
            var fakeMessageSender = new FakeMessageSender();
            var mailCommunicator = new MailCommunicator(fakeMessageSender, new FakeMessageProcessor());

            await mailCommunicator.HandleClientAsync(new FakeTcpClient(), 0, new CancellationToken());

            Assert.AreEqual(Message.Greeting, fakeMessageSender.Message);
        }

        internal class FakeTcpClient : ITcpClient
        {
            public void Dispose()
            {
                
            }

            public Stream GetStream()
            {
                return new MemoryStream(Encoding.ASCII.GetBytes("EHLO\r\n"));
            }
        }

        internal class FakeMessageSender : IMessageSender
        {
            public Message Message { get; private set; }

            public void SendMessage(Stream stream, CancellationToken token, Message greeting)
            {
                Message = greeting;
            }
        }

        internal class FakeMessageProcessor : IMessageProcessor
        {
            public SMTPCommand ProcessMessage(byte[] message)
            {
                return SMTPCommand.Unknown;
            }
        }
    }
}
