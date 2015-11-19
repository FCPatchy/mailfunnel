namespace Mailfunnel.SMTP.Messages.OutboundMessages
{
    public class OutboundMessageClosingTransmission : IOutboundMessage
    {
        public bool Multiline => false;
        public int ReplyCode => 221;

        public string GetMessage()
        {
            return "Mailfunnel service closing transmission channel";
        }
    }
}