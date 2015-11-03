using System;

namespace Mailfunnel.SMTP.Messages
{
    public class MessageProcessor : IMessageProcessor
    {
        /// <summary>
        /// Process an inbound network message.
        /// Returns an object containing the SMTP command and message text, if any.
        /// </summary>
        /// <param name="message">Message to process</param>
        /// <returns></returns>
        public ClientMessage ProcessMessage(string message)
        {
            var clientMessage = new ClientMessage
            {
                MessageText = string.Empty
            };

            var smtpCommand = SmtpCommand.Unknown;

            var setMessageText = false;

            if (message.Length >= 4)
            {
                var command = message.Substring(0, 4);
                if (Enum.TryParse(command, out smtpCommand))
                {
                    if (message.Length > 4)
                    {
                        clientMessage.MessageText = message.Substring(5, message.Length - 5);
                    }
                }
                else
                {
                    setMessageText = true;
                }
            }
            else
            {
                setMessageText = true;
            }

            if (setMessageText)
            {
                // If the command could not be determined from the message,
                // it's either a data payload, or invalid
                clientMessage.MessageText = message;
            }

            clientMessage.SMTPCommand = smtpCommand;

            return clientMessage;
        }

        /// <summary>
        /// Generate a text message from the provided IOutboundMessage implementation.
        /// The generated text message can then be forwarded to the output stream.
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns></returns>
        public string GenerateMessage(IOutboundMessage message)
        {
            return string.Format("{0} {1}\r\n", message.ReplyCode, message.GetMessage());
        }
    }
}