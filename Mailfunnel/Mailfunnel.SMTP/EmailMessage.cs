﻿using System.Collections.Generic;

namespace Mailfunnel.SMTP
{
    public class EmailMessage
    {
        private IList<string> _recipients;

        public string Sender { get; set; }

        public IList<string> Recipients
        {
            get { return _recipients ?? (_recipients = new List<string>()); }
            set { _recipients = value; }
        }

        public string Message { get; set; }
    }
}