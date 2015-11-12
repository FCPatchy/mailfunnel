using Mailfunnel.Data.Entities;
using Mailfunnel.Data.Repository;
using Mailfunnel.SMTP;
using Mailfunnel.SMTP.Clients;

namespace Mailfunnel
{
    public class EmailRecorder : IEmailRecorder
    {
        private readonly IEmailRepository _emailRepository;
        private readonly ISmtpServer _smtpServer;

        public EmailRecorder(IEmailRepository emailRepository, ISmtpServer smtpServer)
        {
            _emailRepository = emailRepository;
            _smtpServer = smtpServer;

            RecordMail();
        }

        public void RecordMail()
        {
            _smtpServer.MessageReceived += MessageReceived;
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            _emailRepository.Add(new EmailEntity
            {
                From = e.Message.Sender,
                Recipients = e.Message.Recipients,
                MessageBody = e.Message.Message,
                Subject = e.Message.Subject,
                Date = e.Message.Date.DateTime
            });
        }
    }
}