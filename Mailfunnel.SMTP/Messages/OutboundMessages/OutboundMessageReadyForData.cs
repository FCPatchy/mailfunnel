namespace Mailfunnel.SMTP.Messages.OutboundMessages
{
    public class OutboundMessageReadyForData : IOutboundMessage
    {
        public bool Multiline => false;
        public int ReplyCode => 354;

        public string GetMessage()
        {
            return "Start mail input; end with <CRLF>.<CRLF>";
        }
    }
}