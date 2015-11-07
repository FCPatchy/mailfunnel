using Nancy;

namespace Mailfunnel.Web.Modules
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = _ => Response.AsFile("app/index.html", "text/html");
        }
    }
}