using Mailfunnel.Web.Dto;
using Mailfunnel.Web.Managers;
using Nancy;

namespace Mailfunnel.Web.Modules
{
    public class AppModule : NancyModule
    {
        public AppModule(IEmailManager emailManager, IGroupManager groupManager) : base("/app")
        {
            Get["/mail"] = _ => new MailsResponse { Emails = emailManager.GetAllEmails() };

            Get["/mail/{id}"] = _ => emailManager.GetEmail(_.id);

            Get["/groups"] = _ => new GroupsResponse { Groups = groupManager.GetAllGroups() };
        }
    }
}