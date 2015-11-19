using Mailfunnel.SMTP.Messages;

namespace Mailfunnel.SMTP.Clients
{
    public class OutboundMessageAuthRequired : IOutboundMessage
    {
        public bool Multiline => false;
        public int ReplyCode => 530;
        public string GetMessage()
        {
            return "Authentication required";
        }
    }
}