using System.Threading;
using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.MIME;
using Mailfunnel.SMTP.Network;

namespace Mailfunnel.SMTP.Clients
{
    public class Client
    {
        private IClientState _clientState;

        public Client(ITcpClientAdapter tcpClient, CancellationToken cancellationToken, IMessager messager, IMimeParser mimeParser)
        {
            TcpClient = tcpClient;
            CancellationToken = cancellationToken;
            
            _clientState = new ConnectedState(this, messager, mimeParser);

            // Change to the 'awaiting EHLO' state
            _clientState = new AwaitingEhloCommandState(this, messager, mimeParser);
        }

        public void ChangeState(IClientState state)
        {
            _clientState = state;
        }

        public CancellationToken CancellationToken { get; set; }
        public EmailMessage Message { get; set; }
        public string MessageBuffer { get; set; }
        public string Group { get; set; }

        public int ClientIdentifier => TcpClient.ClientIdentifier;
        public ITcpClientAdapter TcpClient { get; }

        public EmailMessage MessageReceived(ClientMessage clientMessage)
        {
            _clientState.MessageReceived(clientMessage);

            if (Message != null && Message.Complete)
                return Message;

            return null;
        }
    }
}