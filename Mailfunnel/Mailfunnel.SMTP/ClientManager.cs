using System;
using System.Collections.Generic;
using System.Linq;
using Mailfunnel.SMTP.Contracts;

namespace Mailfunnel.SMTP
{
    public class ClientManager : IClientManager
    {
        private readonly IMailCommunicator _mailCommunicator;
        private readonly List<Client> _clients;
        private readonly Action<Client, string>[,] _fsm;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Client states
        /// </summary>
        public enum State
        {
            Connected,
            AwaitingEhloCommand,
            AwaitingMailCommand,
            AwaitingRcptCommand,
            AwaitingDataCommand,
            AwaitingData
        }

        /// <summary>
        /// State change events
        /// </summary>
        public enum Event
        {
            Connected,
            Ehlo,
            Mail,
            Rcpt,
            Data,
            DataTransmission
        }

        public ClientManager(IMailCommunicator mailCommunicator)
        {
            _mailCommunicator = mailCommunicator;

            _clients = new List<Client>();

            // Subscribe to the mail communicator events
            _mailCommunicator.ClientConnected += OnClientConnected;
            _mailCommunicator.ClientMessageReceived += OnClientMessageReceived;

            // Event transitions
            _fsm = new Action<Client, string>[,]
            {   // Connected        // EHLO     // MAIL      // RCPT      // DATA      // Data transmission
                { StateConnected,   null,       null,        null,        null,        null                  }, // Connected
                { null,             StateEhlo,  BadSequence, BadSequence, BadSequence, null                  }, // AwaitingEhloCommand
                { null,             null,       StateMail,   BadSequence, BadSequence, BadSequence           }, // AwaitingMailCommand
                { null,             null,       null,        StateRcpt,   BadSequence, null                  }, // AwaitingRcptCommand
                { null,             null,       null,        StateRcpt,   StateData,   null                  }, // AwaitingDataCommand
                { null,             null,       null,        null,        null,        StateDataTransmission }, // AwaitingData
            };
        }

        private void StateConnected(Client client, string s)
        {
            client.ClientState = State.AwaitingEhloCommand;
            _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.Greeting);
        }

        private void StateEhlo(Client client, string s)
        {
            client.ClientState = State.AwaitingMailCommand;
            _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.OK);
        }

        private void StateMail(Client client, string s)
        {
            client.Message = new EmailMessage
            {
                Sender = s
            };

            client.ClientState = State.AwaitingRcptCommand;
            _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.OK);
        }

        private void StateRcpt(Client client, string s)
        {
            client.Message.Recipients.Add(s);

            client.ClientState = State.AwaitingDataCommand;
            _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.OK);
        }

        private void StateData(Client client, string s)
        {
            client.ClientState = State.AwaitingData;
            _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.SendData);
        }

        private void StateDataTransmission(Client client, string s)
        {
            // Check if the terminating sequence is present
            var lines = s.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Contains("."))
            {
                // End of transmission
                client.ClientState = State.AwaitingMailCommand;
                _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.OK);

                if (MessageReceived != null)
                {
                    MessageReceived(this, new MessageReceivedEventArgs(client.Message));
                }

                return;
            }

            client.Message.Message += s;
        }

        private void BadSequence(Client client, string s)
        {
            _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.BadSequence);
        }

        private void ProcessEvent(Client client, Event e, string commandText = null)
        {
            _fsm[(int)client.ClientState, (int)e].Invoke(client, commandText);
        }

        private void OnClientConnected(object sender, ClientConnectedEventArgs clientConnectedEventArgs)
        {
            var client = new Client
            {
                TcpClient = clientConnectedEventArgs.Client,
                CancellationToken = clientConnectedEventArgs.CancellationToken
            };

            _clients.Add(client);

            ProcessEvent(client, Event.Connected);
        }

        private void OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            var client = _clients.FirstOrDefault(x => x.TcpClient == e.Client);

            var ev = Event.Connected;
            string commandText = null;

            if (client.ClientState == State.AwaitingData)
            {
                // Special case, where we are expecting message data.
                // In this case, there is no SMTP command
                ProcessEvent(client, Event.DataTransmission, e.MessageText);
                return;
            }

            switch (e.Command)
            {
                case SMTPCommand.EHLO:
                    ev = Event.Ehlo;
                    break;

                case SMTPCommand.MAIL:
                    ev = Event.Mail;
                    commandText = SmtpUtilities.ExtractEmailAddress(e.MessageText);
                    break;

                case SMTPCommand.RCPT:
                    ev = Event.Rcpt;
                    commandText = SmtpUtilities.ExtractEmailAddress(e.MessageText);
                    break;

                case SMTPCommand.DATA:
                    ev = Event.Data;
                    commandText = SmtpUtilities.ExtractEmailAddress(e.MessageText);
                    break;
            }

            ProcessEvent(client, ev, commandText);
            //if (!SetState(client, e.Command))
            //{

            //    return;
            //}
            //ProcessState(client, e.MessageText);
        }

        /// <summary>
        /// Attemps to transition the state of a client.
        /// If the client cannot be transitioned (incorrect order of commands),
        /// false is returned.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        //private bool SetState(Client client, SMTPCommand command)
        //{
        //    var valid = false;

        //    switch (command)
        //    {
        //        case SMTPCommand.EHLO:
        //            client.ClientState = ClientState.EHLO;
        //            valid = true;
        //            break;

        //        case SMTPCommand.MAIL:
        //            if (client.ClientState <= ClientState.MAIL)
        //            {
        //                client.ClientState = ClientState.MAIL;
        //                valid = true;
        //            }
        //            break;

        //        case SMTPCommand.RCPT:
        //            if (client.ClientState <= ClientState.RCPT)
        //            {
        //                client.ClientState = ClientState.RCPT;
        //                valid = true;
        //            }
        //            break;

        //            case SMTPCommand.DATA:
        //            if (client.ClientState <= ClientState.DATA)
        //            {
        //                client.ClientState = ClientState.DATA;
        //                valid = true;
        //            }
        //            break;
        //    }

        //    return valid;
        //}

        //private void ProcessState(Client client, string messageText = null)
        //{
        //    switch (client.ClientState)
        //    {
        //        case ClientState.Initial:
        //            _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.Greeting);
        //            break;

        //        case ClientState.EHLO:

        //            client.Message = null;

        //            _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.OK);
        //            break;

        //        case ClientState.MAIL:

        //            var sender = SmtpUtilities.ExtractEmailAddress(messageText);

        //            client.Message = new EmailMessage
        //            {
        //                Sender = sender
        //            };

        //            _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.OK);
        //            break;

        //        case ClientState.RCPT:

        //            var recipient = SmtpUtilities.ExtractEmailAddress(messageText);

        //            client.Message.Recipients.Add(recipient);

        //            _mailCommunicator.SendMessage(client.TcpClient, client.CancellationToken, Message.OK);
        //            break;

        //        case ClientState.DATA:


        //            break;
        //    }
        //}
    }
}
