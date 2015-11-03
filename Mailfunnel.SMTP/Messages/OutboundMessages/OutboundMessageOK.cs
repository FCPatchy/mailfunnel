namespace Mailfunnel.SMTP.Messages.OutboundMessages
{
    public class OutboundMessageOK : IOutboundMessage
    {
        public int ReplyCode => 250;
        public string GetMessage()
        {
            return "OK";
        }
    }
}
