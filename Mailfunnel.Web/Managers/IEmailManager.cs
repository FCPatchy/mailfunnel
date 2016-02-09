using System.Collections.Generic;
using Mailfunnel.Web.Dto;

namespace Mailfunnel.Web.Managers
{
    public interface IEmailManager
    {
        IEnumerable<Email> GetAllEmails();
        Email GetEmail(int id);
    }
}