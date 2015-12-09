using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.MIME;

namespace Mailfunnel.SMTP.Clients
{
    public class AwaitingAuthPasswordState : AbstractClientState
    {
        public AwaitingAuthPasswordState(Client client, IMessager messager, IMimeParser mimeParser) : base(client, messager, mimeParser)
        {
        }

        public override bool ValidMessage(ClientMessage clientMessage)
        {
            // We're expecting a password, so the message is always valid
            return true;
        }

        public override void MessageReceived(ClientMessage clientMessage)
        {
            base.MessageReceived(clientMessage);

            // We don't care about the password
            _messager.SendMessage(_client, new OutboundMessageAuthSuccessful());

            _client.ChangeState(new AwaitingMailCommandState(_client, _messager, _mimeParser));
        }
    }
}
