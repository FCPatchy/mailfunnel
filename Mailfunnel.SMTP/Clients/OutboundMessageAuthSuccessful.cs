using Mailfunnel.SMTP.Messages;

namespace Mailfunnel.SMTP.Clients
{
    public class OutboundMessageAuthSuccessful : IOutboundMessage
    {
        public bool Multiline => false;
        public int ReplyCode => 235;
        public string GetMessage()
        {
            return "Authentication successful";
        }
    }
}