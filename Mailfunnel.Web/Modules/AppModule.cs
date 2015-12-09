using Mailfunnel.Web.Dto;
using Nancy;

namespace Mailfunnel.Web.Modules
{
    public class AppModule : NancyModule
    {
        public AppModule(IDocumentRepository<EmailEntity> emailRepository, IDocumentRepository<GroupEntity> groupRepository) : base("/app")
        {
            Get["/mail"] = _ => new MailsResponse { Emails = emailRepository.GetAll() };

            Get["/mail/{id}"] = _ => emailRepository.Get(_.id);

            Get["/groups"] = _ => new GroupsResponse { Groups = groupRepository.GetAll() };
        }
    }
}