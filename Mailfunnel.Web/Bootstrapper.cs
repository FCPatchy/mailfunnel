using Microsoft.Practices.Unity;
using Nancy.Bootstrappers.Unity;
using Nancy.Conventions;

namespace Mailfunnel.Web
{
    public class Bootstrapper : UnityNancyBootstrapper
    {
        private readonly IUnityContainer _container;

        public Bootstrapper(IUnityContainer container)
        {
            _container = container;
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("assets",
                @"app/assets"));
        }

        protected override IUnityContainer GetApplicationContainer()
        {
            return _container;
        }
    }
}