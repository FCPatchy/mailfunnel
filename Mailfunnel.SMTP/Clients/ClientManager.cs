using System;
using System.Collections.Generic;
using System.Text;
using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.Messages.OutboundMessages;
using Mailfunnel.SMTP.MIME;
using MimeKit.Encodings;

namespace Mailfunnel.SMTP.Clients
{
    public class ClientManager : IClientManager
    {
        /// <summary>
        ///     State change events
        /// </summary>
        public enum Event
        {
            Connected,
            Ehlo,
            Auth,
            AuthPassword,
            Mail,
            Rcpt,
            Data,
            DataTransmission,
            NoOperation,
            Quit
        }

        /// <summary>
        ///     Client states
        /// </summary>
        public enum State
        {
            Connected,
            AwaitingEhloCommand,
            AwaitingAuth,
            AwaitingAuthPassword,
            AwaitingMailCommand,
            AwaitingRcptCommand,
            AwaitingDataCommand,
            AwaitingData,
        }

        private readonly Dictionary<int, Client> _clients;
        private readonly Action<Client, string>[,] _fsm;
        private readonly IMessager _messager;
        private readonly IMimeParser _mimeParser;

        public ClientManager(IMessager messager, IMimeParser mimeParser)
        {
            _messager = messager;
            _mimeParser = mimeParser;

            _clients = new Dictionary<int, Client>();

            // Subscribe to messager events
            _messager.ClientConnected += OnClientConnected;
            _messager.ClientMessageReceived += OnClientMessageReceived;
            _messager.ClientDisconnected += OnClientDisconnected;

            // Event transitions
            _fsm = new Action<Client, string>[,]
            {
                // Connected      // EHLO      // AUTH      // Password     // MAIL       // RCPT                // DATA         // Data transmission     // NOOP         // QUIT
                {StateConnected,  null,        null,        null,           null,         null,                  null,           null,                    NoOperation,    Quit}, // Connected
                {null,            StateEhlo,   BadSequence, BadSequence,    BadSequence,  null,                  null,           null,                    NoOperation,    Quit}, // AwaitingEhloCommand
                {null,            null,        StateAuth,   AuthRequired,   AuthRequired, null,                  null,           null,                    NoOperation,    Quit}, // AwaitingAuth
                {null,            null,        null,        AuthPassword,   null,         null,                  null,           null,                    NoOperation,    Quit}, // AwaitingAuthPassword
                {null,            null,        null,        null,           StateMail,    BadSequence,           null,           null,                    NoOperation,    Quit}, // AwaitingMailCommand
                {null,            null,        null,        null,           null,         StateRcpt,             BadSequence,    null,                    NoOperation,    Quit}, // AwaitingRcptCommand
                {null,            null,        null,        null,           null,         StateRcpt,             StateData,      null,                    NoOperation,    Quit}, // AwaitingDataCommand
                {null,            null,        null,        null,           null,         null,                  null,           StateDataTransmission,   NoOperation,    Quit}  // AwaitingData
            };
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        private void StateConnected(Client client, string s)
        {
            client.ClientState = State.AwaitingEhloCommand;
            _messager.SendMessage(client, new OutboundMessageGreeting());
        }

        private void StateEhlo(Client client, string s)
        {
            client.ClientState = State.AwaitingAuth;
            _messager.SendMessage(client, new OutboundMessageSessionGreeting(s));
            _messager.SendMessage(client, new OutboundMessageAuth());
        }

        private void StateAuth(Client client, string s)
        {
            client.ClientState = State.AwaitingAuthPassword;

            client.Group = Encoding.UTF8.GetString(Convert.FromBase64String(s));

            _messager.SendMessage(client, new OutboundmessageAuthPassword());
        }

        private void AuthPassword(Client client, string s)
        {
            client.ClientState = State.AwaitingMailCommand;

            // We don't care about the password
            _messager.SendMessage(client, new OutboundMessageAuthSuccessful());
        }


        private void StateMail(Client client, string s)
        {
            client.Message = new EmailMessage
            {
                Sender = s,
                Group = client.Group
            };

            client.ClientState = State.AwaitingRcptCommand;
            _messager.SendMessage(client, new OutboundMessageOK());
        }

        private void StateRcpt(Client client, string s)
        {
            client.Message.Recipients.Add(s);

            client.ClientState = State.AwaitingDataCommand;
            _messager.SendMessage(client, new OutboundMessageOK());
        }

        private void StateData(Client client, string s)
        {
            client.ClientState = State.AwaitingData;
            _messager.SendMessage(client, new OutboundMessageReadyForData());
        }

        private void StateDataTransmission(Client client, string s)
        {
            // Check if the terminating sequence is present
            var lines = s.Split(new[] {"\r\n"}, StringSplitOptions.None);

            foreach (var line in lines)
            {
                client.Message.Message += line + "\r\n";

                if (line == ".")
                {
                    // End of transmission
                    client.ClientState = State.AwaitingMailCommand;
                    _messager.SendMessage(client, new OutboundMessageOK());

                    // Parse the message
                    _mimeParser.ParseMessage(client.Message);

                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(client.Message));

                    return;
                }
            }
        }

        private void NoOperation(Client client, string commandText)
        {
            _messager.SendMessage(client, new OutboundMessageOK());
        }

        private void BadSequence(Client client, string s)
        {
            _messager.SendMessage(client, new OutboundMessageBadSequence());
        }

        private void AuthRequired(Client client, string s)
        {
            _messager.SendMessage(client, new OutboundMessageAuthRequired());
        }

        private void Quit(Client client, string s)
        {
            _messager.SendMessage(client, new OutboundMessageClosingTransmission());
            _messager.DisconnectClient(client);
        }

        private void ProcessEvent(Client client, Event e, string commandText = null)
        {
            _fsm[(int) client.ClientState, (int) e].Invoke(client, commandText);
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs args)
        {
            var client = new Client
            {
                TcpClient = args.Client,
                CancellationToken = args.CancellationToken
            };

            _clients.Add(client.TcpClient.ClientIdentifier, client);

            ProcessEvent(client, Event.Connected);
        }

        private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            _clients.Remove(args.ClientIdentifier);
        }

        private void OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            var clientIdentifier = e.Client.ClientIdentifier;
            var client = _clients[clientIdentifier];

            var ev = Event.Connected;
            string commandText = null;

            if (client.ClientState == State.AwaitingData)
            {
                // Special case, where we are expecting message data.
                // In this case, there is no SMTP command
                ProcessEvent(client, Event.DataTransmission, e.ClientMessage.MessageText);
                return;
            }
            if (client.ClientState == State.AwaitingAuthPassword)
            {
                // Special case, expecting password
                // No SMTP command
                ProcessEvent(client, Event.AuthPassword, e.ClientMessage.MessageText);
                return;
            }

            switch (e.ClientMessage.SMTPCommand)
            {
                case SmtpCommand.EHLO:
                    ev = Event.Ehlo;
                    commandText = e.ClientMessage.MessageText;
                    break;

                case SmtpCommand.AUTH:
                    ev = Event.Auth;
                    commandText = SmtpUtilities.ExtractAuthLogin(e.ClientMessage.MessageText);
                    break;

                case SmtpCommand.MAIL:
                    ev = Event.Mail;
                    commandText = SmtpUtilities.ExtractEmailAddress(e.ClientMessage.MessageText);
                    break;

                case SmtpCommand.RCPT:
                    ev = Event.Rcpt;
                    commandText = SmtpUtilities.ExtractEmailAddress(e.ClientMessage.MessageText);
                    break;

                case SmtpCommand.DATA:
                    ev = Event.Data;
                    commandText = SmtpUtilities.ExtractEmailAddress(e.ClientMessage.MessageText);
                    break;

                case SmtpCommand.NOOP:
                    ev = Event.NoOperation;
                    break;

                case SmtpCommand.QUIT:
                    ev = Event.Quit;
                    break;
            }

            ProcessEvent(client, ev, commandText);
        }
    }
}