using System.Collections.Generic;
using Mailfunnel.Data.Models;

namespace Mailfunnel.Data.Repository
{
    public interface IEmailRepository
    {
        IEnumerable<Email> GetAllEmails();
        Email GetEmail(int id);
    }
}
