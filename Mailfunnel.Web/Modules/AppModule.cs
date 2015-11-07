using Nancy;

namespace Mailfunnel.Web.Modules
{
    public class AppModule : NancyModule
    {
        public AppModule() : base("/app")
        {
            Get["/mails"] = _ => Response.AsJson(new
            {
                mails = new[]
                {
                    new
                    {
                        from = "someone@somewhere.com",
                        subject = "hey there",
                        dateReceived = "Thursday, 2:31 PM"
                    }
                }
            });
        }
    }
}