using Newtonsoft.Json;

namespace Mailfunnel.Data.Repository
{
    public interface IDocumentEntity
    {
        int __id { get; set; }

        [JsonIgnore]
        bool SerializeId { get; set; }
        bool ShouldSerialize__id();
    }
}