﻿namespace Mailfunnel.SMTP.Messages.OutboundMessages
{
    public class OutboundMessageOK : IOutboundMessage
    {
        public bool Multiline => false;
        public int ReplyCode => 250;

        public string GetMessage()
        {
            return "OK";
        }
    }
}