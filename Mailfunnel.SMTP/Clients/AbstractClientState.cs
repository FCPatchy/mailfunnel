using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.Messages.OutboundMessages;
using Mailfunnel.SMTP.MIME;

namespace Mailfunnel.SMTP.Clients
{
    public abstract class AbstractClientState : IClientState
    {
        protected Client _client;
        protected IMessager _messager;
        protected IMimeParser _mimeParser;

        protected AbstractClientState(Client client, IMessager messager, IMimeParser mimeParser)
        {
            _client = client;
            _messager = messager;
            _mimeParser = mimeParser;
        }

        public abstract bool ValidMessage(ClientMessage clientMessage);

        public virtual void MessageReceived(ClientMessage clientMessage)
        {
            if (clientMessage.SMTPCommand == SmtpCommand.NOOP)
            {
                _messager.SendMessage(_client, new OutboundMessageOK());
            }

            if (clientMessage.SMTPCommand == SmtpCommand.QUIT)
            {
                _messager.SendMessage(_client, new OutboundMessageClosingTransmission());
                _messager.DisconnectClient(_client);
            }

            if (!ValidMessage(clientMessage))
            {
                _messager.SendMessage(_client, new OutboundMessageBadSequence());
            }
        }
    }
}
