using System;
using System.Collections.Generic;
using System.Linq;
using Mailfunnel.SMTP.Messages;

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
            Mail,
            Rcpt,
            Data,
            DataTransmission
        }

        /// <summary>
        ///     Client states
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

        private readonly List<Client> _clients;
        private readonly Action<Client, string>[,] _fsm;
        private readonly IMessager _messager;

        public ClientManager(IMessager messager)
        {
            _messager = messager;

            _clients = new List<Client>();

            // Subscribe to messager events
            _messager.ClientConnected += OnClientConnected;
            _messager.ClientMessageReceived += OnClientMessageReceived;

            // Event transitions
            _fsm = new Action<Client, string>[,]
            {
                // Connected     // EHLO    // MAIL      // RCPT      // DATA      // Data transmission
                {StateConnected, null, null, null, null, null}, // Connected
                {null, StateEhlo, BadSequence, BadSequence, BadSequence, null}, // AwaitingEhloCommand
                {null, null, StateMail, BadSequence, BadSequence, BadSequence}, // AwaitingMailCommand
                {null, null, null, StateRcpt, BadSequence, null}, // AwaitingRcptCommand
                {null, null, null, StateRcpt, StateData, null}, // AwaitingDataCommand
                {null, null, null, null, null, StateDataTransmission} // AwaitingData
            };
        }

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        private void StateConnected(Client client, string s)
        {
            client.ClientState = State.AwaitingEhloCommand;
            _messager.SendMessage(client, Message.Greeting);
        }

        private void StateEhlo(Client client, string s)
        {
            client.ClientState = State.AwaitingMailCommand;
            _messager.SendMessage(client, Message.OK);
        }

        private void StateMail(Client client, string s)
        {
            client.Message = new EmailMessage
            {
                Sender = s
            };

            client.ClientState = State.AwaitingRcptCommand;
            _messager.SendMessage(client, Message.OK);
        }

        private void StateRcpt(Client client, string s)
        {
            client.Message.Recipients.Add(s);

            client.ClientState = State.AwaitingDataCommand;
            _messager.SendMessage(client, Message.OK);
        }

        private void StateData(Client client, string s)
        {
            client.ClientState = State.AwaitingData;
            _messager.SendMessage(client, Message.SendData);
        }

        private void StateDataTransmission(Client client, string s)
        {
            // Check if the terminating sequence is present
            var lines = s.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (lines.Contains("."))
                {
                    // End of transmission
                    client.ClientState = State.AwaitingMailCommand;
                    _messager.SendMessage(client, Message.OK);

                    if (MessageReceived != null)
                    {
                        MessageReceived(this, new MessageReceivedEventArgs(client.Message));
                    }

                    return;
                }

                client.Message.Message += line + "\r\n";
            }
        }

        private void BadSequence(Client client, string s)
        {
            _messager.SendMessage(client, Message.BadSequence);
        }

        private void ProcessEvent(Client client, Event e, string commandText = null)
        {
            _fsm[(int) client.ClientState, (int) e].Invoke(client, commandText);
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
                //ProcessEvent(Client, Event.DataTransmission, e.MessageText);
                return;
            }

            //switch (e.Command)
            //{
            //    case SMTPCommand.EHLO:
            //        ev = Event.Ehlo;
            //        break;

            //    case SMTPCommand.MAIL:
            //        ev = Event.Mail;
            //        commandText = SmtpUtilities.ExtractEmailAddress(e.MessageText);
            //        break;

            //    case SMTPCommand.RCPT:
            //        ev = Event.Rcpt;
            //        commandText = SmtpUtilities.ExtractEmailAddress(e.MessageText);
            //        break;

            //    case SMTPCommand.DATA:
            //        ev = Event.Data;
            //        commandText = SmtpUtilities.ExtractEmailAddress(e.MessageText);
            //        break;
            //}

            ProcessEvent(client, ev, commandText);
        }
    }
}