using System.Net;
using Mailfunnel.Data.Entities;
using Mailfunnel.Data.Repository;
using Mailfunnel.SMTP;
using Mailfunnel.SMTP.Clients;
using Mailfunnel.SMTP.Logging;
using Mailfunnel.SMTP.Messages;
using Mailfunnel.SMTP.MIME;
using Mailfunnel.SMTP.Network;
using Mailfunnel.Web;
using Microsoft.Practices.Unity;
using Nancy.Bootstrapper;

namespace Mailfunnel
{
    public class Bootstrapper
    {
        public static IUnityContainer InitialiseContainer()
        {
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IInitialiser, Initialiser>();
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IEmailRecorder, EmailRecorder>();
            container.RegisterType<IWebServer, WebServer>();
            container.RegisterType<ISmtpServer, SmtpServer>();
            container.RegisterType<INancyBootstrapper, Web.Bootstrapper>();
            container.RegisterType<ITcpListenerAdapter, NetworkTcpListener>(new ContainerControlledLifetimeManager(), new InjectionConstructor(IPAddress.Any, 25));
            container.RegisterType<IDocumentRepository<EmailEntity>, EmailRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDocumentRepository<GroupEntity>, GroupRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMimeParser, MimeParser>();
            container.RegisterType<IMessager, Messager>();
            container.RegisterType<IClientManager, ClientManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMessageProcessor, MessageProcessor>();
            container.RegisterType<INetworkMessager, NetworkMessager>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}