using System.Net;
using System.Net.Sockets;
using Mailfunnel.SMTP;
using Mailfunnel.SMTP.Contracts;
using Microsoft.Practices.Unity;

namespace Mailfunnel
{
    public class Bootstrapper
    {
        public static IUnityContainer InitialiseContainer()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IServer, Server>();
            container.RegisterType<ITcpListenerAdapter, NetworkTcpListener>(new InjectionConstructor(IPAddress.Any, 25));
            container.RegisterType<IClientManager, ClientManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageProcessor, MessageProcessor>();
            container.RegisterType<IMailCommunicator, MailCommunicator>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageSender, MessageSender>();

            return container;
        }
    }
}
