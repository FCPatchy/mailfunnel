namespace Mailfunnel.SMTP.Messages.OutboundMessages
{
    /// <summary>
    ///     Message sent out when the client
    ///     sends the EHLO command
    /// </summary>
    public class OutboundMessageSessionGreeting : IOutboundMessage
    {
        private readonly string _clientIdentifier;

        public bool Multiline => true;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="clientIdentifier">Client identifier. Usually a machine name, domain name or IP address.</param>
        public OutboundMessageSessionGreeting(string clientIdentifier)
        {
            _clientIdentifier = clientIdentifier;
        }

        public int ReplyCode => 250;

        public string GetMessage()
        {
            return string.Format("Mailfunnel greets {0}", _clientIdentifier);
        }
    }
}