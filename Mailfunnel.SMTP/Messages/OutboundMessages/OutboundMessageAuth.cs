namespace Mailfunnel.SMTP.Messages.OutboundMessages
{
    public class OutboundMessageAuth : IOutboundMessage
    {
        public bool Multiline => false;
        public int ReplyCode => 250;
        public string GetMessage()
        {
            return "AUTH LOGIN";
        }
    }
}
