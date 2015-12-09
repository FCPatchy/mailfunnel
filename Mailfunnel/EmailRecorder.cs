using Mailfunnel.SMTP;
using Mailfunnel.SMTP.Clients;

namespace Mailfunnel
{
    public class EmailRecorder : IEmailRecorder
    {
        private readonly ISmtpServer _smtpServer;

        public EmailRecorder(ISmtpServer smtpServer)
        {
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

            }
        }
    }
}