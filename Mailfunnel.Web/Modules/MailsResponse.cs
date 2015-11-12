using System.Collections.Generic;
using Mailfunnel.Data.Entities;

namespace Mailfunnel.Web.Modules
{
    public class MailsResponse
    {
        public IEnumerable<EmailEntity> Emails { get; set; }
    }
}