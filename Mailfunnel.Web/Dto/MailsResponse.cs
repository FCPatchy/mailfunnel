using System.Collections.Generic;

namespace Mailfunnel.Web.Dto
{
    public class MailsResponse
    {
        public IEnumerable<Email> Emails { get; set; }
    }

    public class Email
    {
    }
}