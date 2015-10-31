using Mailfunnel.SMTP;
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
