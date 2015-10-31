using Mailfunnel.SMTP;
using Microsoft.Practices.Unity;

namespace Mailfunnel
{
    public class Bootstrapper
    {
        public static IUnityContainer InitialiseContainer()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IServer, Server>();
            container.RegisterType<IMessageProcessor, MessageProcessor>();
            container.RegisterType<IMailCommunicator, MailCommunicator>();
            container.RegisterType<IMessageSender, MessageSender>();

            return container;
        }
    }
}
