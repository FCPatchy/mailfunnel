using Mailfunnel.Data.Models;
using Mailfunnel.Data.Repository;
using Mailfunnel.SMTP;
using Mailfunnel.SMTP.Clients;

namespace Mailfunnel
{
    public class EmailRecorder : IEmailRecorder
    {
        private readonly ISmtpServer _smtpServer;
        private readonly IEmailRepository _emailRepository;
        private readonly IGroupRepository _groupRepository;

        public EmailRecorder(ISmtpServer smtpServer, IEmailRepository emailRepository, IGroupRepository groupRepository)
        {
            _smtpServer = smtpServer;
            _emailRepository = emailRepository;
            _groupRepository = groupRepository;

            RecordMail();
        }

        public void RecordMail()
        {
            _smtpServer.MessageReceived += MessageReceived;
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var groupId = 0;
            // Is there a group?
            if (!string.IsNullOrWhiteSpace(e.Message.Group))
            {
                var group = _groupRepository.GetGroup(e.Message.Group);
                if (group != null)
                    groupId = group.Id;
            }

            _emailRepository.SetEmail(new Email
            {
                From = e.Message.Sender,
                To = string.Join(";", e.Message.Recipients),
                Subject = e.Message.Subject,
                BodyHtml = e.Message.Message,
                GroupId = groupId
            });
        }
    }
}