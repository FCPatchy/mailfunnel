using System;
using System.Text;
using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.MIME;

namespace Mailfunnel.SMTP.Clients
{
    public class AwaitingAuthState : AbstractClientState
    {
        public AwaitingAuthState(Client client, IMessager messager, IMimeParser mimeParser) : base(client, messager, mimeParser)
        {
        }

        public override bool ValidMessage(ClientMessage clientMessage)
        {
            return clientMessage.SMTPCommand == SmtpCommand.AUTH;
        }

        public override void MessageReceived(ClientMessage clientMessage)
        {
            base.MessageReceived(clientMessage);

            var message = SmtpUtilities.ExtractAuthLogin(clientMessage.MessageText);

            _client.Group = Encoding.UTF8.GetString(Convert.FromBase64String(message));
            _messager.SendMessage(_client, new OutboundMessageAuthPassword());

            _client.ChangeState(new AwaitingAuthPasswordState(_client, _messager, _mimeParser));
        }
    }
}
