using System;

namespace Mailfunnel.SMTP.Network
{
    public class NetworkClientDisconnectedEventArgs : EventArgs
    {
        public NetworkClientDisconnectedEventArgs(int clientHashCode)
        {
            ClientHashCode = clientHashCode;
        }

        public int ClientHashCode { get; private set; }
    }
}