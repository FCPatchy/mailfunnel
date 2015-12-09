using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.Messages.OutboundMessages;
using Mailfunnel.SMTP.MIME;

namespace Mailfunnel.SMTP.Clients
{
    public class AwaitingMailCommandState : AbstractClientState
    {
        public AwaitingMailCommandState(Client client, IMessager messager, IMimeParser mimeParser) : base(client, messager, mimeParser)
        {
        }

        public override bool ValidMessage(ClientMessage clientMessage)
        {
            return clientMessage.SMTPCommand == SmtpCommand.MAIL;
        }

        public override void MessageReceived(ClientMessage clientMessage)
        {
            base.MessageReceived(clientMessage);

            var sender = SmtpUtilities.ExtractEmailAddress(clientMessage.MessageText);

            _client.Message = new EmailMessage
            {
                Sender = sender,
                Group = _client.Group
            };

            _messager.SendMessage(_client, new OutboundMessageOK());

            _client.ChangeState(new AwaitingRcptCommandState(_client, _messager, _mimeParser));
        }
    }
}
