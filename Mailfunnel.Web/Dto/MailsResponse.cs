using System.Collections.Generic;
using Mailfunnel.Data.Entities;

namespace Mailfunnel.Web.Dto
{
    public class MailsResponse
    {
        public IEnumerable<EmailEntity> Emails { get; set; }
    }
}