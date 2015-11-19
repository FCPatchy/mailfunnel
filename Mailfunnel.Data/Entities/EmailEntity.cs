using System;
using System.Collections.Generic;
using Mailfunnel.Data.Repository;

namespace Mailfunnel.Data.Entities
{
    public class EmailEntity : IDocumentEntity
    {
        public int __id { get; set; }
        public string From { get; set; }
        public IList<string> Recipients { get; set; }
        public string MessageBody { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Group { get; set; }
    }
}