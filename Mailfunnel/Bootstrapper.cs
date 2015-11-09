using System.Net;
using Mailfunnel.SMTP;
using Mailfunnel.SMTP.Clients;
using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.Network;
using Microsoft.Practices.Unity;

namespace Mailfunnel
{
    public class Bootstrapper
    {
        public static IUnityContainer InitialiseContainer()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<ISmtpServer, SmtpServer>();
            container.RegisterType<ITcpListenerAdapter, NetworkTcpListener>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(IPAddress.Any, 25));
            container.RegisterType<IMessager, Messager>();
            container.RegisterType<IClientManager, ClientManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageProcessor, MessageProcessor>();
            container.RegisterType<INetworkMessager, NetworkMessager>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}