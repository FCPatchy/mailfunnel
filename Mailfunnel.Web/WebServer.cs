using System;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;

namespace Mailfunnel.Web
{
    public class WebServer : IWebServer
    {
        private readonly NancyHost _nancyHost;

        public WebServer(INancyBootstrapper bootstrapper)
        {
            _nancyHost = new NancyHost(new Uri("http://localhost:1234"), bootstrapper, new HostConfiguration
            {
                RewriteLocalhost = false
            });
        }

        public void Start()
        {
            _nancyHost.Start();
        }

        public void Stop()
        {
            _nancyHost.Stop();
        }
    }
}