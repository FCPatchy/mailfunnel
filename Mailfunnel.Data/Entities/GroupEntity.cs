using Mailfunnel.Data.Repository;
using Newtonsoft.Json;

namespace Mailfunnel.Data.Entities
{
    public class GroupEntity : IDocumentEntity
    {
        public int __id { get; set; }
        [JsonIgnore]
        public bool SerializeId { get; set; }

        public bool ShouldSerialize__id()
        {
            return SerializeId;
        }

        public string Name { get; set; }
    }
}
