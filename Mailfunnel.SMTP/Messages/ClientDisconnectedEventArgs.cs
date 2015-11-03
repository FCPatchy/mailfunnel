namespace Mailfunnel.SMTP.Messages
{
    public class ClientDisconnectedEventArgs
    {
        public ClientDisconnectedEventArgs(int clientIdentifier)
        {
            ClientIdentifier = clientIdentifier;
        }

        public int ClientIdentifier { get; private set; }
    }
}