using System.Net;
using Mailfunnel.SMTP;
using Mailfunnel.SMTP.Contracts;
using Microsoft.Practices.Unity;

namespace Mailfunnel
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Bootstrapper.InitialiseContainer();

            var server = container.Resolve<IServer>();
            
            server.Listen();
        }
    }
}
