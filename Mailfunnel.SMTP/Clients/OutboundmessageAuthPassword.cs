using Mailfunnel.SMTP.Messages;

namespace Mailfunnel.SMTP.Clients
{
    public class OutboundmessageAuthPassword : IOutboundMessage
    {
        public bool Multiline => false;
        public int ReplyCode => 334;
        public string GetMessage()
        {
            return "UGFzc3dvcmQ6"; // Base64 encoded string 'Password:'
        }
    }
}