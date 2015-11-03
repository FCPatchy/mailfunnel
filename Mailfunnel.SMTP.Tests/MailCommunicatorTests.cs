using System;
using System.IO;
using System.Text;
using System.Threading;
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

            public int ClientIdentifier { get; }

            public Stream GetStream()
            {
                return new MemoryStream(Encoding.ASCII.GetBytes("EHLO\r\n"));
            }

            public void Close()
            {
                throw new NotImplementedException();
            }
        }

        internal class FakeMessageProcessor : IMessageProcessor
        {
            public ClientMessage ProcessMessage(string message)
            {
                throw new NotImplementedException();
            }

            public string GenerateMessage(IOutboundMessage message)
            {
                throw new NotImplementedException();
            }

            public string GenerateMessage(Message message)
            {
                throw new NotImplementedException();
            }

            public SmtpCommand ProcessMessage(byte[] message)
            {
                return SmtpCommand.Unknown;
            }
        }

        [Test]
        public async void HandleClientAsync_SendsWelcomeMessage()
        {
            var mailCommunicator = new NetworkMessager();

            await mailCommunicator.HandleClientAsync(new FakeTcpClient(), 0, new CancellationToken());

            //Assert.AreEqual(Message.Greeting, fakeMessageSender.Message);
        }
    }
}