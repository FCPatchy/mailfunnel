using Mailfunnel.SMTP;
using Mailfunnel.Web;

namespace Mailfunnel
{
    public class Initialiser : IInitialiser
    {
        private readonly IEmailRecorder _emailRecorder;
        private readonly ISmtpServer _smtpServer;
        private readonly IWebServer _webServer;

        public Initialiser(IWebServer webServer, ISmtpServer smtpServer, IEmailRecorder emailRecorder)
        {
            _webServer = webServer;
            _smtpServer = smtpServer;
            _emailRecorder = emailRecorder;
        }

        public void Initialise()
        {
            _webServer.Start();
            _smtpServer.Listen();
        }
    }
}