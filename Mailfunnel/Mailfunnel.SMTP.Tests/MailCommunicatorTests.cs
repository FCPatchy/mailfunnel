using System;
using System.IO;
using System.Text;
using System.Threading;
using Mailfunnel.SMTP.Contracts;
using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.Network;
using NUnit.Framework;

namespace Mailfunnel.SMTP.Tests
{
    [TestFixture]
    public class MailCommunicatorTests
    {
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
            public ClientMessage ProcessMessage(string message)
            {
                throw new NotImplementedException();
            }

            public SMTPCommand ProcessMessage(byte[] message)
            {
                return SMTPCommand.Unknown;
            }
        }

        [Test]
        public async void HandleClientAsync_SendsWelcomeMessage()
        {
            var fakeMessageSender = new FakeMessageSender();
            var mailCommunicator = new NetworkMessager();

            await mailCommunicator.HandleClientAsync(new FakeTcpClient(), 0, new CancellationToken());

            Assert.AreEqual(Message.Greeting, fakeMessageSender.Message);
        }
    }
}