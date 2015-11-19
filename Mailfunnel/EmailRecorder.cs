using Mailfunnel.Data.Entities;
using Mailfunnel.Data.Repository;
using Mailfunnel.SMTP;
using Mailfunnel.SMTP.Clients;

namespace Mailfunnel
{
    public class EmailRecorder : IEmailRecorder
    {
        private readonly IDocumentRepository<EmailEntity> _emailRepository;
        private readonly IDocumentRepository<GroupEntity> _groupRepository;
        private readonly ISmtpServer _smtpServer;

        public EmailRecorder(IDocumentRepository<EmailEntity> emailRepository, IDocumentRepository<GroupEntity> groupRepository, ISmtpServer smtpServer)
        {
            _emailRepository = emailRepository;
            _groupRepository = groupRepository;
            _smtpServer = smtpServer;

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
                var group = new GroupEntity
                {
                    Name = e.Message.Group
                };

                // Ensure it exists
                _groupRepository.Ensure(group, x => x.Name == group.Name);
            }

            _emailRepository.Add(new EmailEntity
            {
                From = e.Message.Sender,
                Recipients = e.Message.Recipients,
                MessageBody = e.Message.Message,
                Subject = e.Message.Subject,
                Date = e.Message.Date.DateTime,
                Group = e.Message.Group
            });
        }
    }
}