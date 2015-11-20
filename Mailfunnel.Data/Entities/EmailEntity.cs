using System;
using System.Collections.Generic;
using Mailfunnel.Data.Repository;
using Newtonsoft.Json;

namespace Mailfunnel.Data.Entities
{
    public class EmailEntity : IDocumentEntity
    {
        public int __id { get; set; }
        [JsonIgnore]
        public bool SerializeId { get; set; }

        public bool ShouldSerialize__id()
        {
            return SerializeId;
        }

        public string From { get; set; }
        public IList<string> Recipients { get; set; }
        public string MessageBody { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public int GroupId { get; set; }
    }
}