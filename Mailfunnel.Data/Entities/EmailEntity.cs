using Mailfunnel.Data.Repository;

namespace Mailfunnel.Data.Entities
{
    public class EmailEntity : IDocumentEntity
    {
        public string Name { get; set; }
        public int __id { get; set; }
    }
}