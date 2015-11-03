using System;

namespace Mailfunnel.SMTP.Messages
{
    public class MessageProcessor : IMessageProcessor
    {
        public ClientMessage ProcessMessage(string message)
        {
            var clientMessage = new ClientMessage
            {
                MessageText = string.Empty
            };

            var smtpCommand = SMTPCommand.Unknown;

            if (message.Length >= 4)
            {
                var command = message.Substring(0, 4);
                if (Enum.TryParse(command, out smtpCommand) && message.Length > 4)
                {
                    clientMessage.MessageText = message.Substring(5, message.Length - 5);
                }
                else
                {
                    // If the command could not be determined from the message,
                    // it's either a data payload, or invalid
                    clientMessage.MessageText = message;
                }
            }

            clientMessage.SMTPCommand = smtpCommand;

            return clientMessage;
        }
    }
}