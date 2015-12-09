using Mailfunnel.SMTP;

namespace Mailfunnel
{
    public class Initialiser : IInitialiser
    {
        private readonly IEmailRecorder _emailRecorder;
        private readonly ISmtpServer _smtpServer;

        public Initialiser(ISmtpServer smtpServer, IEmailRecorder emailRecorder)
        {
            _smtpServer = smtpServer;
            _emailRecorder = emailRecorder;
        }

        public void Initialise()
        {
            _smtpServer.Listen();
        }
    }
}