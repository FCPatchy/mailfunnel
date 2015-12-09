using Mailfunnel.SMTP.Messages;

namespace Mailfunnel.SMTP.Clients
{
    public interface IClientState
    {
        void MessageReceived(ClientMessage clientMessage);
    }
}