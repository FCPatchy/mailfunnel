using System;
using System.Collections.Generic;

namespace Mailfunnel.SMTP
{
    public class EmailMessage
    {
        public bool Complete { get; set; }
        private IList<string> _recipients;

        public string Sender { get; set; }

        public IList<string> Recipients
        {
            get { return _recipients ?? (_recipients = new List<string>()); }
            set { _recipients = value; }
        }

        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Group { get; set; }
    }
}