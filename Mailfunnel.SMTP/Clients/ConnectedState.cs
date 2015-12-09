using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.Messages.OutboundMessages;
using Mailfunnel.SMTP.MIME;

namespace Mailfunnel.SMTP.Clients
{
    public class ConnectedState : AbstractClientState
    {
        public ConnectedState(Client client, IMessager messager, IMimeParser mimeParser) : base(client, messager, mimeParser)
        {
            // Send a greeting immediately upon client connection
            _messager.SendMessage(_client, new OutboundMessageGreeting());
        }

        public override bool ValidMessage(ClientMessage clientMessage)
        {
            // Messages are never received in this state
            return true;
        }
    }
}
