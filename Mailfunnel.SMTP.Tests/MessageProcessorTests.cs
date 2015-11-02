using Mailfunnel.SMTP.Messages;
using NUnit.Framework;

namespace Mailfunnel.SMTP.Tests
{
    [TestFixture]
    internal class MessageProcessorTests
    {
        [Test]
        public void ProcessMessage_extracts_command_with_message()
        {
            const string rawMessage = "EHLO client.example.com";

            var msgProcessor = new MessageProcessor();

            var msg = msgProcessor.ProcessMessage(rawMessage);

            Assert.AreEqual(SMTPCommand.EHLO, msg.SMTPCommand);
            Assert.AreEqual("client.example.com", msg.MessageText);
        }

        [Test]
        public void ProcessMessage_returns_message_when_no_command_given()
        {
            const string rawMessage = "From: sender@example.com\r\nTo: recipient@example.com\r\n";

            var msgProcessor = new MessageProcessor();

            var msg = msgProcessor.ProcessMessage(rawMessage);

            Assert.AreEqual(SMTPCommand.Unknown, msg.SMTPCommand);
            Assert.AreEqual(rawMessage, msg.MessageText);
        }

        [Test]
        public void ProcessMessage_Returns_Message_When_Message_Too_Short_For_Command()
        {
            const string rawMessage = "ab";

            var msgProcessor = new MessageProcessor();

            var msg = msgProcessor.ProcessMessage(rawMessage);

            Assert.AreEqual(SMTPCommand.Unknown, msg.SMTPCommand);
            Assert.AreEqual(rawMessage, msg);
        }

        [Test]
        public void ProcessMessage_Returns_No_Message_When_Command_Only()
        {
            const string rawMessage = "EHLO";

            var msgProcessor = new MessageProcessor();

            var msg = msgProcessor.ProcessMessage(rawMessage);

            Assert.AreEqual(SMTPCommand.EHLO, msg.SMTPCommand);
            Assert.AreEqual(string.Empty, msg.MessageText);
        }
    }
}