using Mailfunnel.Data.Repository;
using Nancy;

namespace Mailfunnel.Web.Modules
{
    public class AppModule : NancyModule
    {
        public AppModule(IEmailRepository emailRepository) : base("/app")
        {
            Get["/mails"] = _ => Response.AsJson(new MailsResponse { Emails = emailRepository.GetAll() });
        }
    }
}