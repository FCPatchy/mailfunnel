namespace Mailfunnel.SMTP.Messages.OutboundMessages
{
    /// <summary>
    ///     Message sent out when client
    ///     establishes a connection to the server
    /// </summary>
    public class OutboundMessageGreeting : IOutboundMessage
    {
        public int ReplyCode => 220;

        public string GetMessage()
        {
            return "Mailfunnel service ready";
        }
    }
}