﻿using System;
using System.Text;
using Mailfunnel.SMTP.Contracts;

namespace Mailfunnel.SMTP
{
    public class MessageProcessor : IMessageProcessor
    {
        public SMTPCommand ProcessMessage(byte[] message)
        {
            var msg = Encoding.ASCII.GetString(message);
            Console.WriteLine("Client says: " + msg.Replace('\r', ' ').Replace('\n', ' '));

            return ExtractCommand(msg);
        }

        private static SMTPCommand ExtractCommand(string msg)
        {
            var smtpCommand = SMTPCommand.Unknown;

            if (msg.Length >= 4)
            {
                var command = msg.Substring(0, 4);
                Enum.TryParse(command, out smtpCommand);
            }

            return smtpCommand;
        }
    }
}
