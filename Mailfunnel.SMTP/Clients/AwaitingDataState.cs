using System;
using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.Messages.OutboundMessages;
using Mailfunnel.SMTP.MIME;

namespace Mailfunnel.SMTP.Clients
{
    public class AwaitingDataState : AbstractClientState
    {
        public AwaitingDataState(Client client, IMessager messager, IMimeParser mimeParser) : base(client, messager, mimeParser)
        {
        }

        public override bool ValidMessage(ClientMessage clientMessage)
        {
            // We're expecting data, so an SMTP command cannot be determined
            return clientMessage.SMTPCommand == SmtpCommand.Unknown;
        }

        public override void MessageReceived(ClientMessage clientMessage)
        {
            base.MessageReceived(clientMessage);

            // Todo - the actual parsing of the message shouldn't really happen here...

            // Check if the terminating sequence is present
            var lines = clientMessage.MessageText.Split(new[] { "\r\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                _client.Message.Message += line + "\r\n";

                if (line == ".")
                {
                    // End of transmission
                    
                    _messager.SendMessage(_client, new OutboundMessageOK());

                    // Parse the message
                    _mimeParser.ParseMessage(_client.Message);

                    // Full message has been formed
                    _client.Message.Complete = true;

                    _client.ChangeState(new AwaitingMailCommandState(_client, _messager, _mimeParser));

                    return;
                }
            }
        }
    }
}
