using Mailfunnel.Data.Entities;
using Mailfunnel.Data.Repository;
using Nancy;

namespace Mailfunnel.Web.Modules
{
    public class AppModule : NancyModule
    {
        public AppModule(IDocumentRepository<EmailEntity> emailRepository) : base("/app")
        {
            Get["/mail"] = _ => new MailsResponse { Emails = emailRepository.GetAll() };

            Get["/mail/{id}"] = _ => emailRepository.Get(_.id);
        }
    }
}