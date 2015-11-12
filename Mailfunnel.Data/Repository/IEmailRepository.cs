using System.Collections.Generic;
using Mailfunnel.Data.Entities;

namespace Mailfunnel.Data.Repository
{
    public interface IEmailRepository : IDocumentRepository<EmailEntity>
    {
        IEnumerable<EmailEntity> GetAll();
    }
}