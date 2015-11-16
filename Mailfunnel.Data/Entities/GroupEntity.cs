using Mailfunnel.Data.Repository;

namespace Mailfunnel.Data.Entities
{
    public class GroupEntity : IDocumentEntity
    {
        public int __id { get; set; }
        public string Name { get; set; }
    }
}
