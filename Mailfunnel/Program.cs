using System;
using Mailfunnel.SMTP;
using Mailfunnel.Web;
using Microsoft.Practices.Unity;

namespace Mailfunnel
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Web server
            var webServer = new WebServer();
            webServer.Start();

            // SMTP server
            var container = Bootstrapper.InitialiseContainer();
            var server = container.Resolve<ISmtpServer>();
            server.Listen();


            Console.ReadLine();
        }
    }
}