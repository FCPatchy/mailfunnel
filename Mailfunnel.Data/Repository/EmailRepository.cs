using Mailfunnel.Data.Entities;

namespace Mailfunnel.Data.Repository
{
    public class EmailRepository : DocumentRepositoryBase<EmailEntity>, IDocumentRepository<EmailEntity>
    {
        public override string Collection
        {
            get { return "Emails"; }
        }
    }
}