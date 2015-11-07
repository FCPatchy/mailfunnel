using System;
using Nancy.Hosting.Self;

namespace Mailfunnel.Web
{
    public class WebServer
    {
        private readonly NancyHost _nancyHost;

        public WebServer()
        {
            var bootstrapper = new Bootstrapper();

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