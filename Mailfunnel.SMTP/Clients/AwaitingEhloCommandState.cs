using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.Messages.OutboundMessages;
using Mailfunnel.SMTP.MIME;

namespace Mailfunnel.SMTP.Clients
{
    public class AwaitingEhloCommandState : AbstractClientState
    {
        public AwaitingEhloCommandState(Client client, IMessager messager, IMimeParser mimeParser) : base(client, messager, mimeParser)
        {
        }

        public override bool ValidMessage(ClientMessage clientMessage)
        {
            return clientMessage.SMTPCommand == SmtpCommand.EHLO;
        }

        public override void MessageReceived(ClientMessage clientMessage)
        {
            base.MessageReceived(clientMessage);
            
            _messager.SendMessage(_client, new OutboundMessageSessionGreeting(clientMessage.MessageText));
            _messager.SendMessage(_client, new OutboundMessageAuth());

            _client.ChangeState(new AwaitingAuthState(_client, _messager, _mimeParser));
        }
    }
}