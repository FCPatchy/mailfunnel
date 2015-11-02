using System.Net;
using Mailfunnel.SMTP;
using Mailfunnel.SMTP.Clients;
using Mailfunnel.SMTP.Contracts;
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

            container.RegisterType<IServer, Server>();
            container.RegisterType<ITcpListenerAdapter, NetworkTcpListener>(new InjectionConstructor(IPAddress.Any, 25));
            container.RegisterType<IClientManager, ClientManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageProcessor, MessageProcessor>();
            container.RegisterType<INetworkMessager, NetworkMessager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageSender, MessageSender>();

            return container;
        }
    }
}