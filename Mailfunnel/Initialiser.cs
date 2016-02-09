using Mailfunnel.Data.Infrastructure;
using Mailfunnel.SMTP;
using Mailfunnel.Web;

namespace Mailfunnel
{
    public class Initialiser : IInitialiser
    {
        private readonly IDatabaseInitialiser _databaseInitialiser;
        private readonly IWebServer _webServer;
        private readonly IEmailRecorder _emailRecorder;
        private readonly ISmtpServer _smtpServer;

        public Initialiser(IDatabaseInitialiser databaseInitialiser, IWebServer webServer, ISmtpServer smtpServer, IEmailRecorder emailRecorder)
        {
            _databaseInitialiser = databaseInitialiser;
            _webServer = webServer;
            _smtpServer = smtpServer;
            _emailRecorder = emailRecorder;
        }

        public void Initialise()
        {
            _databaseInitialiser.InitialiseDatabase();
            _webServer.Start();
            _smtpServer.Listen();
        }
    }
}