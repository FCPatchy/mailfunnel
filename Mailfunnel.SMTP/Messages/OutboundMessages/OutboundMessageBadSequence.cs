namespace Mailfunnel.SMTP.Messages.OutboundMessages
{
    public class OutboundMessageBadSequence : IOutboundMessage
    {
        public int ReplyCode => 503;

        public string GetMessage()
        {
            return "Bad sequence of commands";
        }
    }
}