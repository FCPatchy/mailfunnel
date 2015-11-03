using Mailfunnel.SMTP;
using Microsoft.Practices.Unity;

namespace Mailfunnel
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = Bootstrapper.InitialiseContainer();

            var server = container.Resolve<IServer>();

            server.Listen();
        }
    }
}