using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.Messages.OutboundMessages;
using Mailfunnel.SMTP.MIME;

namespace Mailfunnel.SMTP.Clients
{
    public class AwaitingRcptCommandState : AbstractClientState
    {
        public AwaitingRcptCommandState(Client client, IMessager messager, IMimeParser mimeParser) : base(client, messager, mimeParser)
        {
        }

        public override bool ValidMessage(ClientMessage clientMessage)
        {
            return clientMessage.SMTPCommand == SmtpCommand.RCPT;
        }

        public override void MessageReceived(ClientMessage clientMessage)
        {
            base.MessageReceived(clientMessage);

            _client.Message.Recipients.Add(SmtpUtilities.ExtractEmailAddress(clientMessage.MessageText));

            _messager.SendMessage(_client, new OutboundMessageOK());

            _client.ChangeState(new AwaitingDataCommandState(_client, _messager, _mimeParser));
        }
    }
}
