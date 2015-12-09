using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.Messages.OutboundMessages;
using Mailfunnel.SMTP.MIME;

namespace Mailfunnel.SMTP.Clients
{
    public class AwaitingDataCommandState : AbstractClientState
    {
        public AwaitingDataCommandState(Client client, IMessager messager, IMimeParser mimeParser) : base(client, messager, mimeParser)
        {
        }

        public override bool ValidMessage(ClientMessage clientMessage)
        {
            return clientMessage.SMTPCommand == SmtpCommand.DATA;
        }

        public override void MessageReceived(ClientMessage clientMessage)
        {
            base.MessageReceived(clientMessage);
            
            _messager.SendMessage(_client, new OutboundMessageReadyForData());

            _client.ChangeState(new AwaitingDataState(_client, _messager, _mimeParser));
        }
    }
}
